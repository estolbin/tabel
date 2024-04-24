
using System.Collections.Generic;

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
        var periods = await _repository.GetAllPeriods();
        return  periods.LastOrDefault(x => !x.Closed);
    }

    public async Task<IEnumerable<TimeShiftPeriod>> GetAllPeriods()
    {
        return await _repository.GetAllPeriods();
    }

    public async Task<TimeShiftPeriod> GetPeriodByDate(DateTime date, CancellationToken token = default)
    {
        return await _repository.GetTimeShiftsPeriodByDate(date);
    }

    public async Task<TimeShiftPeriod> GetLastPeriod(CancellationToken token = default)
    {
        var period =  await _repository.GetLastPeriod();
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

    public async Task<IEnumerable<TimeShift>> GetCurrentTimeShift(CancellationToken token = default)
    {
        var period = _repository.GetLastPeriod().Result;
        var list = await _repository.GetTimeShiftsByPeriod(period);
        return list;
    }

    public async Task<Employee> GetEmployeeById(Guid id)
    {
        return await _repository.GetEmployeeById(id);
    }

    public async Task<TimeShift> GetTimeShiftByEmpAndDate(Guid id, DateTime date, CancellationToken token = default)
    {
        return await _repository.GetTimeShiftByEmpDate(id, date);
    }

    public void UpdateTimeShift(TimeShift timeShift)
    {
        _repository.UpdateTimeShift(timeShift);

    }


    public async Task<TimeShift> GetTimeShiftByID(Guid id) 
    {
        return await _repository.GetTimeShiftById(id);
    }

    public async Task<IEnumerable<Department>> GetAllDepartments()
    {
        return await  _repository.GetAllDepartments();
    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftsByDepartment(Guid departmentId)
    {
        return await _repository.GetTimeShiftByDepartment(departmentId); 
    }

    public async Task<IEnumerable<Organization>> GetAllOrganization()
    {
        return await _repository.GetAllOrganizations();
    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftByOrganization(Guid organizationId)
    {
        return await _repository.GetTimeShiftByOrganization(organizationId);
    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftByEmpLike(string empLike)
    {
        return await _repository.GetTimeShiftByEmpLike(empLike);
    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftByDepartments(List<Guid> depsGuids)
    {
        return await _repository.GetTimeShiftByDepartments(depsGuids);
    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftByOrganizations(List<Guid> orgGuids)
    {
        return await _repository.GetTimeShiftByOrganizations(orgGuids);
    }
}

