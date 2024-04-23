using Microsoft.EntityFrameworkCore;
using web_tabel.Domain;
using web_tabel.Services.TimeShiftContext;

namespace web_tabel.Services;

public class TimeShiftRepository : ITimeShiftRepository
{
    private readonly TimeShiftDBContext _context;

    public TimeShiftRepository(TimeShiftDBContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<TimeShift>> GetTimeShiftsByPeriod(TimeShiftPeriod period)
    {
        return await _context.TimeShifts
            .Include(ts => ts.Employee)
                .ThenInclude(emp => emp.Name)
            .Include(ts => ts.Employee)
                .ThenInclude(emp => emp.StaffSchedule)
                    .ThenInclude(s => s.Position)
             .Include(ts => ts.Employee)
                .ThenInclude(w => w.TypeOfEmployment)
            .Include(ts => ts.TypeEmployment)
            .Include(ts => ts.WorkSchedule)
            .OrderBy(ts => ts.WorkDate)
            .Where(x => x.TimeShiftPeriod == period)
            .ToListAsync();
    }

    public async Task<TimeShiftPeriod> GetTimeShiftsPeriodByDate(DateTime date)
    {
        var found = _context.TimeShiftPeriods.FirstOrDefaultAsync(x => x.Start <= date && x.End >= date);
        if (found is null)
            throw new KeyNotFoundException();
        return found.Result;
    }

    public async Task<IEnumerable<TimeShiftPeriod>> GetAllPeriods()
    {
        return await _context.TimeShiftPeriods.ToListAsync();
    }

    public async Task<TimeShiftPeriod> GetLastPeriod()
    {
        //var found = _context.TimeShiftPeriods.Order().LastOrDefaultAsync();
        // TODO: написать правильный метод запроса по получению последнего периода
        var t = _context.TimeShiftPeriods.ToList()[0];
        return t;
    }

    public async void AddPeriod(TimeShiftPeriod period)
    {
        _context.TimeShiftPeriods.Add(period);
        _context.SaveChangesAsync();
    }

    public async void RemoveTimeShiftPeriodByID(Guid Id)
    {
        var period = _context.TimeShiftPeriods.FirstOrDefault(x => x.Id == Id);
        if (period != null)
        {
            _context.TimeShiftPeriods.Remove(period);
            _context.SaveChangesAsync();
        }
    }

    public async Task<Employee> GetEmployeeById(Guid Id)
    {
        var emp = _context.Employees
            .Include(emp => emp.Name)
            .Include(e => e.Organization)
            .Include(e => e.Department)
            .Include(e => e.StaffSchedule)
                .ThenInclude(ss => ss.Position)
            .FirstOrDefault(x => x.Id == Id);
        return emp;
    }

    public Task<TimeShift> GetTimeShiftByEmpDate(Guid id, DateTime date)
    {

        var res = _context.TimeShifts
            .Include(e => e.Employee)
            .ThenInclude(n => n.Name)
            .FirstOrDefaultAsync(t => t.WorkDate == date && t.Employee.Id == id);

        return res;
    }

    public async void UpdateTimeShift(TimeShift timeShift)
    {
        _context.TimeShifts.Update(timeShift);
        _context.SaveChangesAsync();
    }

    public async Task<TimeShift> GetTimeShiftById(Guid id)
    {
        var res = await _context.TimeShifts.FirstOrDefaultAsync(x => x.Id == id);
        return res;
    }

    public async Task<IEnumerable<Department>> GetAllDepartments()
    {
        return await _context.Departments
            .Include(e => e.Organization)
            .ToListAsync();
    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftByDepartment(Guid departmentId)
    {
        List<Employee> emps = GetEmployeesByDepId(departmentId);

        IQueryable<TimeShift> ts = GetTimeShiftByEmployeeList(emps);

        return ts;

    }

    private IQueryable<TimeShift> GetTimeShiftByEmployeeList(List<Employee> emps)
    {
        return _context.TimeShifts
            .Include(e => e.Employee)
               .ThenInclude(n => n.Name)
            .Include(ts => ts.Employee)
                .ThenInclude(emp => emp.StaffSchedule)
                    .ThenInclude(s => s.Position)
            .Include(ts => ts.Employee)
                .ThenInclude(e => e.TypeOfEmployment)
            .Include(ts => ts.WorkSchedule)
            .Include(tp => tp.TypeEmployment)
            .Where(e => emps.Contains(e.Employee));
    }

    private List<Employee> GetEmployeesByDepId(Guid departmentId)
    {
        return _context.Employees.Where(x => x.Department.Id == departmentId).ToList();
    }

    public async Task<IEnumerable<Organization>> GetAllOrganizations()
    {
        return await _context.Organizations.ToListAsync();
    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftByOrganization(Guid organizationId)
    {
        List<Employee> emp = GetEmployeesByOrgId(organizationId);
        var ts = GetTimeShiftByEmployeeList(emp);
        return ts;
    }

    private List<Employee> GetEmployeesByOrgId(Guid organizationId)
    {
        return _context.Employees.Where(x => x.Organization.Id == organizationId).ToList();
    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftByEmpLike(string empLike)
    {
        var names = _context.EmployeeNames.AsEnumerable().Where(x => x.FullName.ToLower().Contains(empLike.ToLower()));
        if (names == null) return null;
        var emps = _context.Employees.Where(e => names.Contains(e.Name)).ToList();
        var ts = GetTimeShiftByEmployeeList(emps);
        return ts;
    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftByDepartments(List<Guid> depsGuids)
    {
        List<TimeShift> ts = new List<TimeShift>();
        foreach ( var dep in depsGuids) 
        {
            var temp = GetTimeShiftByDepartment(dep).Result;
            foreach (var item in temp)
            {
                ts.Add(item);
            }

        }
        return ts;

    }

    public async Task<IEnumerable<TimeShift>> GetTimeShiftByOrganizations(List<Guid> orgGuids)
    {
        var ts = new List<TimeShift>();
        foreach (var org in orgGuids)
        {
            var temp = GetTimeShiftByOrganization(org).Result;
            foreach (var item in temp)
            {
                ts.Add(item);
            }
        }
        return ts;
    }
}