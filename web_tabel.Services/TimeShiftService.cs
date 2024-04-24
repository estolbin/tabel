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

    public async Task<IEnumerable<TimeShift>> GetCurrentTimeShift(CancellationToken token = default)
    {

        var period = unitOfWork.TimeShiftPeriodRepository.SingleOrDefault(x => !x.Closed);
        return unitOfWork.TimeShiftRepository.Get(x => x.TimeShiftPeriod == period);

        //var period = _repository.GetLastPeriod().Result;
        //var list = await _repository.GetTimeShiftsByPeriod(period);
        //return list;
    }

    public async Task<Employee> GetEmployeeById(Guid id)
    {
        //return await _repository.GetEmployeeById(id);
        return unitOfWork.EmployeeRepository.Get(x => x.Id == id).FirstOrDefault();
    }

    public async Task<TimeShift> GetTimeShiftByEmpAndDate(Guid id, DateTime date, CancellationToken token = default)
    {
        //return await _repository.GetTimeShiftByEmpDate(id, date);
        var employee = unitOfWork.EmployeeRepository.Get(x => x.Id == id).FirstOrDefault();
        var period = unitOfWork.TimeShiftPeriodRepository.Get(x => x.Start <= date && x.End >= date).LastOrDefault();
        return unitOfWork.TimeShiftRepository.Get(x => x.Employee == employee && x.TimeShiftPeriod == period).LastOrDefault();
    }

    public void UpdateTimeShift(TimeShift timeShift)
    {
        //_repository.UpdateTimeShift(timeShift);
        unitOfWork.TimeShiftRepository.Update(timeShift);

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
    /// ����� �� �����, ������ � ��������
    /// </summary>
    /// <param name="empLike"></param>
    /// <returns>������ �����������, � ������� ���� ����������</returns>
    public async Task<IEnumerable<TimeShift>> GetTimeShiftByEmpLike(string empLike)
    {
        var names = unitOfWork.EmployeeNameRepository.GetAll().AsEnumerable().Where(x => x.FullName.ToLower().Contains(empLike.ToLower()));
        var employees = unitOfWork.EmployeeRepository.Get(x => names.Contains(x.Name));
        return unitOfWork.TimeShiftRepository.Get(e => employees.Contains(e.Employee));
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
