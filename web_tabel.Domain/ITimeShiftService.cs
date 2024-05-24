namespace web_tabel.Domain;

public interface ITimeShiftService
{
    public Task<IEnumerable<TimeShift>> GetTimeShifts(TimeShiftPeriod period, CancellationToken token = default);
    public Task<TimeShiftPeriod> GetLastUnclosedPeriod(CancellationToken token = default);
    public Task<TimeShiftPeriod> GetPeriodByDate(DateTime date, CancellationToken token = default);
    public Task<TimeShiftPeriod> GetLastPeriod(CancellationToken token = default);
    public Task<IEnumerable<TimeShiftPeriod>> GetAllPeriods();
    bool RemovePeriodById(Guid id);

    void AddPeriod(TimeShiftPeriod period);

    public Task<IEnumerable<TimeShift>> GetCurrentTimeShift(Guid? periodId, CancellationToken token = default);
    public Task<Employee> GetEmployeeById(Guid id);

    public Task<TimeShift> GetTimeShiftByEmpAndDate(Guid employeeId, DateTime date, Guid? periodId, CancellationToken token = default);

    public Task UpdateTimeShift(TimeShift timeShift);

    public Task<TimeShift> GetTimeShiftByID(Guid id);

    public Task<IEnumerable<Department>> GetAllDepartments();

    public Task<IEnumerable<TimeShift>> GetTimeShiftsByDepartment(Guid departmentId);

    public Task<IEnumerable<Organization>> GetAllOrganization();

    public Task<IEnumerable<TimeShift>> GetTimeShiftByOrganization(Guid organizationId);

    public Task<IEnumerable<TimeShift>> GetTimeShiftByEmpLike(string empLike);

    public Task<IEnumerable<TimeShift>> GetTimeShiftByDepartments(List<Guid> depsGuids);
    public Task<IEnumerable<TimeShift>> GetTimeShiftByOrganizations(List<Guid> orgGuids);

    public Task<TypeOfWorkingTime> GetTypeOfWorkingTime(string name);

    public Task<IEnumerable<TypeOfWorkingTime>> GetAllTypeOfWorkingTime();
}