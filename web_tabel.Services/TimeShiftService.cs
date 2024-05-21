using web_tabel.Services;

namespace web_tabel.Domain;

public class TimeShiftService : ITimeShiftService
{
    // TODO: refactor for use generic repository
    //private readonly ITimeShiftRepository _repository;
    private readonly UnitOfWork unitOfWork = new();

    //public TimeShiftService(ITimeShiftRepository repository)
    //{
    //    ArgumentNullException.ThrowIfNull(repository);
    //    _repository = repository;
    //}

    public async Task<IEnumerable<TimeShift>> GetTimeShifts(TimeShiftPeriod period, CancellationToken token = default)
    {
        //var result = await _repository.GetTimeShiftsByPeriod(period);
        var result = unitOfWork.TimeShiftRepository.Get(t => t.TimeShiftPeriod == period);
        return result;
    }

    public async Task<TimeShiftPeriod> GetLastUnclosedPeriod(CancellationToken token = default)
    {
        //var periods = await _repository.GetAllPeriods();
        //return periods.LastOrDefault(x => !x.Closed);
        return unitOfWork.TimeShiftPeriodRepository.Get(x => !x.Closed).LastOrDefault();
    }

    public async Task<IEnumerable<TimeShiftPeriod>> GetAllPeriods()
    {
        //return await _repository.GetAllPeriods();
        return unitOfWork.TimeShiftPeriodRepository.GetAll();
    }

    public async Task<TimeShiftPeriod> GetPeriodByDate(DateTime date, CancellationToken token = default)
    {
        //return await _repository.GetTimeShiftsPeriodByDate(date);
        return unitOfWork.TimeShiftPeriodRepository.Get(x => x.Start <= date && x.End >= date).LastOrDefault();
    }

    public async Task<TimeShiftPeriod> GetLastPeriod(CancellationToken token = default)
    {
        return  unitOfWork.TimeShiftPeriodRepository.Get(x => x.Name != "").LastOrDefault();
        //var period = await _repository.GetLastPeriod();
        //if (period.Name.ToString() == "")
        //{
        //    _repository.RemoveTimeShiftPeriodByID(period.Id);
        //    period = new TimeShiftPeriod(name: "Default", start: DateTime.Parse("01.04.2024"), end: DateTime.Parse("14.04.2024"), closed: false);
        //    _repository.AddPeriod(period);
        //}
        //return period;
    }

    public bool RemovePeriodById(Guid Id)
    {
        var period = unitOfWork.TimeShiftPeriodRepository.Get(x => x.Id == Id).FirstOrDefault();
        unitOfWork.TimeShiftPeriodRepository.Delete(period);

        return true;

        //_repository.RemoveTimeShiftPeriodByID(Id);
        //return true;
    }

    public void AddPeriod(TimeShiftPeriod period)
    {
        //_repository.AddPeriod(period);
        unitOfWork.TimeShiftPeriodRepository.Insert(period);

    }


    // TODO Refactor this method
    public async Task<IEnumerable<TimeShift>> GetCurrentTimeShift(Guid? periodId = null, CancellationToken token = default)
    {
        IEnumerable<TimeShiftPeriod> periods = new List<TimeShiftPeriod>();
        if (periodId == null || periodId == Guid.Empty)
        {
            periods = unitOfWork.TimeShiftPeriodRepository.GetAll();
        } else
        {
            periods = unitOfWork.TimeShiftPeriodRepository.Get(x => x.Id == periodId);
        }

        var period = periods.OrderByDescending(x => x.End).FirstOrDefault();
        return unitOfWork.TimeShiftRepository.Get(x => x.TimeShiftPeriod == period);
    }

    public async Task<Employee> GetEmployeeById(Guid id)
    {
        //return await _repository.GetEmployeeById(id);
        return unitOfWork.EmployeeRepository.Get(x => x.Id == id).FirstOrDefault();
    }

    public async Task<TimeShift> GetTimeShiftByEmpAndDate(Guid id, DateTime date, Guid? periodId = null,  CancellationToken token = default)
    {
        //return await _repository.GetTimeShiftByEmpDate(id, date);
        var employee = unitOfWork.EmployeeRepository.Get(x => x.Id == id).FirstOrDefault();
        TimeShiftPeriod period;
        if (periodId == null || periodId == Guid.Empty)
        {
            period = unitOfWork.TimeShiftPeriodRepository.Get(x => x.Start <= date && x.End >= date).LastOrDefault();
        } else
        {
            period = unitOfWork.TimeShiftPeriodRepository.SingleOrDefault(x => x.Id == periodId);
        }
        return unitOfWork.TimeShiftRepository.Get(x => x.Employee == employee && x.TimeShiftPeriod == period && x.WorkDate == date).LastOrDefault();
    }

    public async Task UpdateTimeShift(TimeShift timeShift)
    {
        //_repository.UpdateTimeShift(timeShift);
        await unitOfWork.TimeShiftRepository.UpdateAsync(timeShift);
        await unitOfWork.SaveAsync();
    }


    public async Task<TimeShift> GetTimeShiftByID(Guid id)
    {
        //return await _repository.GetTimeShiftById(id);
        return unitOfWork.TimeShiftRepository.Get(x => x.Id == id).FirstOrDefault();
    }

    public async Task<IEnumerable<Department>> GetAllDepartments()
    {
     //   return await _repository.GetAllDepartments();
        return unitOfWork.DepartmentRepository.GetAll();
    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftsByDepartment(Guid departmentId)
    {
        //return await _repository.GetTimeShiftByDepartment(departmentId);
        var department = unitOfWork.DepartmentRepository.Get(x => x.Id == departmentId).FirstOrDefault();
        return unitOfWork.TimeShiftRepository.Get(x => x.Employee.Department == department);
    }

    public async Task<IEnumerable<Organization>> GetAllOrganization()
    {
        //return await _repository.GetAllOrganizations();
        return unitOfWork.OrganizationRepository.GetAll();
    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftByOrganization(Guid organizationId)
    {
        //return await _repository.GetTimeShiftByOrganization(organizationId);
        var organization = unitOfWork.OrganizationRepository.Get(x => x.Id == organizationId).FirstOrDefault();
        return unitOfWork.TimeShiftRepository.Get(x => x.Employee.Organization == organization);
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
        //return await _repository.GetTimeShiftByDepartments(depsGuids);
        return unitOfWork.TimeShiftRepository.Get(x => depsGuids.Contains(x.Employee.Department.Id));
    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftByOrganizations(List<Guid> orgGuids)
    {
        //return await _repository.GetTimeShiftByOrganizations(orgGuids);
        return unitOfWork.TimeShiftRepository.Get(x => orgGuids.Contains(x.Employee.Organization.Id));
    }
}

