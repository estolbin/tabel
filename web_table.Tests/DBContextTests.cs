using Microsoft.EntityFrameworkCore;
using web_tabel.Domain;
using web_tabel.Services;
using web_tabel.Services.TimeShiftContext;

namespace web_table.Tests;

public class DBContextTests : IDisposable
{
    private readonly TimeShiftDBContext _context;
    private readonly TimeShiftRepository _repo;

    public DBContextTests()
    {
        var options = new DbContextOptionsBuilder<TimeShiftDBContext>()
            .UseInMemoryDatabase(databaseName: "TimeShiftDB")
            .Options;
        _context = new TimeShiftDBContext(options);
        _context.Database.EnsureCreated();

        _repo = new TimeShiftRepository(_context);
    }

    private Guid orgGuid = new Guid("C5DCDC10-1AD6-4F76-AC8A-BE6E4F169AAA");
    private Guid depGuid = new Guid("5AFEFC11-D0D6-4D1A-AEAB-7607CD8E2C04");
    private Guid tsGuid = new Guid("2B35821C-225A-4522-9E5D-1DE410E652A4");

    private void SeedData()
    {

        var org = new Organization()
        {
            Id = orgGuid,
            Name = "Test",
        };

        var dep = new Department("Test", org);
        var pos = new Position("Test", org, dep);
        var workSchedule = new WorkSchedule()
        {
            Id = depGuid,
            Name = "Test",
            HoursOfWork = 40f
        };
        var staff = new StaffSchedule(org, dep, pos, workSchedule, "Test");

        var name = new EmployeeName("Иванов Иван Иванович");
        var emp = new Employee(name, org, dep, staff);
        emp.TypeEmployment = new TypeOfWorkingTime("РВ", "Рабочее время");
        emp.WorkSchedule = workSchedule;

        _context.Organizations.Add(org);
        _context.Departments.Add(dep);
        _context.Positions.Add(pos);
        _context.WorkSchedules.Add(workSchedule);
        _context.StaffSchedules.Add(staff);
        _context.EmployeeNames.Add(name);
        _context.Employees.Add(emp);


        var per = new TimeShiftPeriod("Новый период", DateTime.Parse("01.04.2024"), DateTime.Parse("15.04.2024"));
        _context.TimeShiftPeriods.Add(per);

        List<TimeShift> tsper = new();
        for (DateTime date = per.Start; date <= per.End; date = date.AddDays(1))
        {
            var t = new TimeShift(per, emp, date);
            tsper.Add(t);
        }
        _context.TimeShifts.AddRange(tsper);

        _context.SaveChanges();
    }

    [Fact]
    public void Should_Create_Organization()
    {
        var org = new Organization()
        {
            Id = orgGuid,
            Name = "Test",
        };

        _context.Organizations.Add(org);

        _context.SaveChanges();

        Assert.Equal(1, _context.Organizations.Count());
        Assert.Equal("Test", _context.Organizations.First().Name);
        Assert.Equal(orgGuid, _context.Organizations.First().Id);

    }

    [Fact]
    public void Should_Create_Department()
    {
        var org = new Organization()
        {
            Id = orgGuid,
            Name = "Test",
        };

        var dep = new Department("Test", org);

        _context.Organizations.Add(org);
        _context.Departments.Add(dep);

        _context.SaveChanges();

        Assert.Equal(1, _context.Departments.Count());
        Assert.Equal("Test", _context.Departments.First().Name);
    }

    [Fact]
    public void Should_Create_Position()
    {
        var org = new Organization()
        {
            Id = orgGuid,
            Name = "Test",
        };

        var dep = new Department("Test", org);

        var pos = new Position("Test", org, dep);

        _context.Organizations.Add(org);
        _context.Departments.Add(dep);
        _context.Positions.Add(pos);

        _context.SaveChanges();

        Assert.Equal(1, _context.Positions.Count());
        Assert.Equal(dep, _context.Positions.First().Department);
        Assert.Equal("Test", _context.Positions.First().Name);
    }


    [Fact]
    public void Should_Create_AllBase()
    {
        SeedData();

        var org = _context.Organizations.First();
        Assert.Equal(org.Id, orgGuid);

        Assert.Equal(1, _context.Departments.Count());
        Assert.Equal("Test", _context.Departments.First().Name);

        Assert.Equal(1, _context.Positions.Count());
        Assert.Equal("Test", _context.Positions.First().Name);

        Assert.Equal(1, _context.WorkSchedules.Count());
        Assert.Equal("Test", _context.WorkSchedules.First().Name);
    }

    [Fact]
    public void Should_Have_Workday()
    {
        SeedData();

        var hours = _context.TimeShifts.Where(x => x.WorkDate == DateTime.Parse("03.04.2024")).Select(x => x.HoursPlanned).FirstOrDefault();

        Assert.Equal(8f, hours);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}