namespace web_tabel.Domain;

public interface ITimeShiftRepository
{
    public Task<IEnumerable<TimeShift>> GetTimeShiftsByPeriod(TimeShiftPeriod period);
    public Task<TimeShiftPeriod> GetTimeShiftsPeriodByDate(DateTime date);
    public Task<IEnumerable<TimeShiftPeriod>> GetAllPeriods();
    
    public Task<TimeShiftPeriod> GetLastPeriod();


    public void AddPeriod(TimeShiftPeriod period);
    public void RemoveTimeShiftPeriodByID(Guid id);
    public Task<Employee> GetEmployeeById(Guid id);

    public Task<TimeShift> GetTimeShiftByEmpDate(Guid id,  DateTime date);

    public void UpdateTimeShift(TimeShift timeShift);

    public Task<TimeShift> GetTimeShiftById(Guid id);

    public Task<IEnumerable<Department>> GetAllDepartments();

    public Task<IEnumerable<TimeShift>> GetTimeShiftByDepartment(Guid departmentId);
    public Task<IEnumerable<Organization>> GetAllOrganizations();

    public Task<IEnumerable<TimeShift>> GetTimeShiftByOrganization(Guid organizationId);

    public Task<IEnumerable<TimeShift>> GetTimeShiftByEmpLike(string  empLike);
}