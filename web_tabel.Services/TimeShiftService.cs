using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using web_tabel.Domain.UserFilters;
using web_tabel.Services;

namespace web_tabel.Domain;

public class TimeShiftService : ITimeShiftService
{
    // TODO: refactor for use generic repository
    //private readonly ITimeShiftRepository _repository;
    private readonly UnitOfWork unitOfWork = new();
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TimeShiftService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    //public TimeShiftService(ITimeShiftRepository repository)
    //{
    //    ArgumentNullException.ThrowIfNull(repository);
    //    _repository = repository;
    //}

    public async Task<IEnumerable<TimeShift>> GetTimeShifts(TimeShiftPeriod period, CancellationToken token = default)
    {
        var result = await unitOfWork.TimeShiftRepository.GetAsync(t => t.TimeShiftPeriod == period);
        return result;
    }

    public async Task<TimeShiftPeriod> GetLastUnclosedPeriod(CancellationToken token = default)
    {
        return await unitOfWork.TimeShiftPeriodRepository.SingleOrDefaultAsync(x => !x.Closed);
    }

    public async Task<IEnumerable<TimeShiftPeriod>> GetAllPeriods()
    {
        return await unitOfWork.TimeShiftPeriodRepository.GetAllAsync();
    }

    public async Task<TimeShiftPeriod> GetPeriodByDate(DateTime date, CancellationToken token = default)
    {
        return await unitOfWork.TimeShiftPeriodRepository.SingleOrDefaultAsync(x => x.Start <= date && x.End >= date);
    }

    public static async Task<TimeShiftPeriod> GetLastPeriod(UnitOfWork unitOfWork, CancellationToken token = default)
    {
        return  await unitOfWork.TimeShiftPeriodRepository.SingleOrDefaultAsync(x => x.Name != "");
    }

    public async Task<bool> RemovePeriodById(Guid Id)
    {
        var period = await unitOfWork.TimeShiftPeriodRepository.SingleOrDefaultAsync(x => x.Id == Id);
        await unitOfWork.TimeShiftPeriodRepository.DeleteAsync(period);
        return true;
    }

    public async Task AddPeriod(TimeShiftPeriod period)
    {
        await unitOfWork.TimeShiftPeriodRepository.InsertAsync(period);
    }

    // TODO Refactor this method
    public async Task<IEnumerable<TimeShift>> GetCurrentTimeShift(Guid? periodId = null, CancellationToken token = default)
    {
        IEnumerable<TimeShiftPeriod> periods = new List<TimeShiftPeriod>();
        if (periodId == null || periodId == Guid.Empty)
        {
            periods = await unitOfWork.TimeShiftPeriodRepository.GetAllAsync();
        } else
        {
            periods = await unitOfWork.TimeShiftPeriodRepository.GetAsync(x => x.Id == periodId);
        }

        var period = periods.OrderByDescending(x => x.End).FirstOrDefault();
        return await unitOfWork.TimeShiftRepository.GetAsync(x => x.TimeShiftPeriod == period);
    }

    public async Task<Employee> GetEmployeeById(Guid id)
    {
        return await unitOfWork.EmployeeRepository.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TimeShift> GetTimeShiftByEmpAndDate(Guid id, DateTime date, Guid? periodId = null,  CancellationToken token = default)
    {
        var employee = await unitOfWork.EmployeeRepository.SingleOrDefaultAsync(x => x.Id == id);
        TimeShiftPeriod period;
        if (periodId == null || periodId == Guid.Empty)
        {
            period = await unitOfWork.TimeShiftPeriodRepository.SingleOrDefaultAsync(x => x.Start <= date && x.End >= date);
        } else
        {
            period = await unitOfWork.TimeShiftPeriodRepository.SingleOrDefaultAsync(x => x.Id == periodId);
        }
        return await unitOfWork.TimeShiftRepository.SingleOrDefaultAsync(x => x.Employee == employee && x.TimeShiftPeriod == period && x.WorkDate == date);
    }

    public async Task UpdateTimeShift(TimeShift timeShift)
    {
        await unitOfWork.TimeShiftRepository.UpdateAsync(timeShift);
        await unitOfWork.SaveAsync();
    }


    public async Task<TimeShift> GetTimeShiftByID(Guid id)
    {
        return await unitOfWork.TimeShiftRepository.SingleOrDefaultAsync(x => x.Id == id);
    }


    private async Task<AppUser> GetCurrentUser()
    {
        var userInClaims = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if(int.TryParse(userInClaims.Value, out int userId))
        {
            return await unitOfWork.UserRepository.SingleOrDefaultAsync(x => x.Id == userId);
        }
        return null;
    }

    public async Task<IEnumerable<Department>> GetAllDepartments()
    {
        var currentUser = await GetCurrentUser();
        var curFilter = currentUser.Filter;

        if (curFilter == null) return await unitOfWork.DepartmentRepository.GetAllAsync();

        List<Guid> deps = null;

        if (curFilter.FilterType == "Department" || curFilter.FilterType == "Composite")
        {
            deps = (curFilter as DepartmentFilter).DepartmentIds;
            return await unitOfWork.DepartmentRepository.GetAsync(x => deps.Contains(x.Id));
        }
        if (deps == null) return await unitOfWork.DepartmentRepository.GetAllAsync();
        else
        {
            return await unitOfWork.DepartmentRepository.GetAsync(x => deps.Contains(x.Id));
        }
    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftsByDepartment(Guid departmentId)
    {
        var department =  await unitOfWork.DepartmentRepository.SingleOrDefaultAsync(x => x.Id == departmentId);
        return await unitOfWork.TimeShiftRepository.GetAsync(x => x.Employee.Department == department);
    }

    public async Task<IEnumerable<Organization>> GetAllOrganization()
    {
        return await unitOfWork.OrganizationRepository.GetAllAsync();
    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftByOrganization(Guid organizationId)
    {
        var organization = await unitOfWork.OrganizationRepository.SingleOrDefaultAsync(x => x.Id == organizationId);
        return await unitOfWork.TimeShiftRepository.GetAsync(x => x.Employee.Organization == organization);
    }

    /// <summary>
    /// Поиск по имени, фамили и отчеству
    /// </summary>
    /// <param name="empLike"></param>
    /// <returns>Список сотрудников, у которых есть совпадения</returns>
    public async Task<IEnumerable<TimeShift>> GetTimeShiftByEmpLike(string empLike)
    {
        // TODO: rework to async method
        var employees = await unitOfWork.EmployeeRepository.GetAllAsync();
        var filteredEmployees = employees.Where(x => x.Name.FullName.ToLower().Contains(empLike.ToLower()));

        return await unitOfWork.TimeShiftRepository.GetAsync(e => filteredEmployees.Contains(e.Employee));
    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftByDepartments(List<Guid> depsGuids)
    {
        return await unitOfWork.TimeShiftRepository.GetAsync(x => depsGuids.Contains(x.Employee.Department.Id));
    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftByOrganizations(List<Guid> orgGuids)
    {
        return await unitOfWork.TimeShiftRepository.GetAsync(x => orgGuids.Contains(x.Employee.Organization.Id));
    }

    public async Task<TypeOfWorkingTime> GetTypeOfWorkingTime(string name)
    {
        return await unitOfWork.TypeOfWorkingTimeRepository.SingleOrDefaultAsync(x => x.Name == name);
    }

    public async Task<IEnumerable<TypeOfWorkingTime>> GetAllTypeOfWorkingTime()
    {
        return await unitOfWork.TypeOfWorkingTimeRepository.GetAllAsync();
    }


    // TODO: fix interface

    public Task<TimeShiftPeriod> GetLastPeriod(CancellationToken token = default)
    {
        return TimeShiftService.GetLastPeriod(new UnitOfWork());
    }

    bool ITimeShiftService.RemovePeriodById(Guid id) => true;

    void ITimeShiftService.AddPeriod(TimeShiftPeriod period)
    {
        return;
    }
}

