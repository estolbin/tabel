namespace web_tabel.Domain;

public class TimeShiftService : ITimeShiftService
{
    private readonly ITimeShiftRepository _repository;

    public TimeShiftService(ITimeShiftRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }
    
    public async Task<IEnumerable<TimeShift>> GetTimeShifts(TimeShiftPeriod period, CancellationToken token = default)
    {
        var result = await _repository.GetTimeShiftsByPeriod(period);
        return result;
    }

    public async Task<TimeShiftPeriod> GetLastUnclosedPeriod(CancellationToken token = default)
    {
        var period = await _repository.GetAllPeriods();
        var result = period.LastOrDefault(x => x.Closed == false);
        return result;
    }

    public async Task<IEnumerable<TimeShiftPeriod>> GetAllPeriods()
    {
        return await _repository.GetAllPeriods();
    }

    public async Task<TimeShiftPeriod> GetPeriodByDate(DateTime date, CancellationToken token = default)
    {
        var period = await _repository.GetTimeShiftsPeriodByDate(date);
        return period;
    }

    public async Task<TimeShiftPeriod> GetLastPeriod(CancellationToken token = default)
    {
        var period = await _repository.GetLastPeriod();
        if (period.Name.ToString() == "")
        {
            _repository.RemoveTimeShiftPeriodByID(period.Id);
            period = new TimeShiftPeriod(name: "Default", start: DateTime.Parse("01.04.2024"), end: DateTime.Parse("14.04.2024"), closed: false);
            _repository.AddPeriod(period);
        }
        return period;
    }

    public bool RemovePeriodById(Guid Id)
    {
        _repository.RemoveTimeShiftPeriodByID(Id);
        return true;
    }

    public void AddPeriod(TimeShiftPeriod period)
    {
        _repository.AddPeriod(period);

    }

    public Task<IEnumerable<TimeShift>> GetCurrentTimeShift(CancellationToken token = default)
    {
        var period = _repository.GetLastPeriod().Result;
        var list = _repository.GetTimeShiftsByPeriod(period);
        return list;
    }

    public Task<Employee> GetEmployeeById(Guid id)
    {
        var res = _repository.GetEmployeeById(id);
        return res;
    }

    public Task<TimeShift> GetTimeShiftByEmpAndDate(Guid id, DateTime date, CancellationToken token = default)
    {
        var res = _repository.GetTimeShiftByEmpDate(id, date);
        return res;
    }

    public void UpdateTimeShift(TimeShift timeShift)
    {
        _repository.UpdateTimeShift(timeShift);

    }


    public Task<TimeShift> GetTimeShiftByID(Guid id) 
    {
        return _repository.GetTimeShiftById(id);
    }

    public Task<IEnumerable<Department>> GetAllDepartments()
    {
        return _repository.GetAllDepartments();
    }

    public Task<IEnumerable<TimeShift>> GetTimeShiftsByDepartment(Guid departmentId)
    {
        return _repository.GetTimeShiftByDepartment(departmentId); 
    }

    public Task<IEnumerable<Organization>> GetAllOrganization()
    {
        return _repository.GetAllOrganizations();
    }

    public Task<IEnumerable<TimeShift>> GetTimeShiftByOrganization(Guid organizationId)
    {
        return _repository.GetTimeShiftByOrganization(organizationId);
    }

    public Task<IEnumerable<TimeShift>> GetTimeShiftByEmpLike(string empLike)
    {
        return _repository.GetTimeShiftByEmpLike(empLike);
    }
}

