using web_tabel.Domain;

namespace web_table.Tests;

public class DomainEntityTest
{
    public Organization TestOrganization = new("Test");

    private Department TestDep()
    {
        return new("Test department", TestOrganization);
    }
    
    [Fact] 
    public void Should_Create_Organization()
    {
        // Arrange
        var organization = new Organization("Test");

        // Act
        
        // Assert
        Assert.NotNull(organization);
        Assert.Equal("Test", organization.Name);
    }

    [Fact]
    public void Should_Create_Department()
    {
        var department = new Department("Test department", TestOrganization);

        Assert.NotNull(department);
        Assert.Equal("Test department", department.Name);
        Assert.Equal("Test", department.Organization.Name);
    }

    [Fact]
    public void Should_Create_Position()
    {
        var position = new Position("Test position", TestOrganization, TestDep());
        
        Assert.NotNull(position);
        Assert.Equal("Test position", position.Name);
        Assert.Equal("Test department", position.Department.Name);
        Assert.Equal("Test", position.Department.Organization.Name);
    }

    [Fact]
    public void Should_Create_WorkSchedule()
    {
        var workSchedule = new WorkSchedule();
        workSchedule.Name = "Пятидневка"; // рабочее время
        workSchedule.HoursOfWork = 40.0f;
        
        Assert.NotNull(workSchedule);
        Assert.Equal("Пятидневка", workSchedule.Name);
        Assert.Equal(40.0f, workSchedule.HoursOfWork);

    }

    [Fact]
    public void Should_Create_TypeEmployment()
    {
        var typeEmployment = new TypeOfWorkingTime();
        typeEmployment.Name = "РB"; // рабочее время
        typeEmployment.Description = "Рабочее время";

        var typeEmployment1 = new TypeOfWorkingTime();
        typeEmployment1.Name = "РB"; // рабочее время
        typeEmployment1.Description = "Еще одно рабочее время";
        
        
        Assert.NotNull(typeEmployment);
        Assert.Equal(typeEmployment, typeEmployment1); // сравнение по наименованию
    }

    [Fact]
    public void Should_Exeption_When_Name_Is_Null()
    {
        var typeEmployment = new TypeOfWorkingTime();
        Assert.Throws<ArgumentNullException>(() => typeEmployment.Name = null);
    }

    [Fact]
    public void Should_Exeption_When_Name_More_Than_Max_Length()
    {
        var typeEmployment = new TypeOfWorkingTime();
        Assert.Throws<ArgumentOutOfRangeException>(() => typeEmployment.Name = new string('*', 10));
    }


    private StaffSchedule TestStaff()
    {
        var position = new Position("Test position", TestOrganization, TestDep());
        WorkSchedule workSchedule = new() 
        {
            Name = "Пятидневка",
            HoursOfWork = 40.0f
        };

        return new StaffSchedule(TestOrganization, TestDep(), position, workSchedule, "Директор/Отдел дирекции");
    }
    
    [Fact]
    public void Should_Create_StaffSchedule()
    {
        var testStaff = TestStaff();
        
        Assert.NotNull(testStaff);
        Assert.Equal("Директор/Отдел дирекции", testStaff.Name);
    }

    [Fact]
    public void Staff_Should_Have_WorkSchedule()
    {
        var testStaff = TestStaff();
        Assert.NotNull(testStaff.WorkSchedule);
        Assert.Equal(40.0f, testStaff.WorkSchedule.HoursOfWork);
    }

    [Fact]
    public void Staff_Should_Have_Position()
    {
        var testStaff = TestStaff();
        testStaff.NumberOfPositions = 10.0f;

        Assert.Equal(10.0f, testStaff.NumberOfPositions);
    }

    [Fact]
    public void Staff_Should_Exception_When_NumberOfPositions_Less_Than_Zero()
    {
        var testStaff = TestStaff();
        Assert.Throws<ArgumentOutOfRangeException>(() => testStaff.NumberOfPositions = -1);
    }

    [Fact]
    public void WorkSchedule_Period_Check_Days_With_Weekend()
    {
        var period = new TimeShiftPeriod("Test", DateTime.Parse("01.04.2024"), DateTime.Parse("14.04.2024"), false);
        List<DateTime> days = period.GetAllDaysInPeriod(true).ToList();
        Assert.Equal(14, days.Count);
    }
    
    [Fact]
    public void WorkSchedule_Period_Check_Days_Without_Weekend()
    {
        var period = new TimeShiftPeriod("Test", DateTime.Parse("01.04.2024"), DateTime.Parse("14.04.2024"), false);
        List<DateTime> days = period.GetAllDaysInPeriod().ToList();
        Assert.Equal(10, days.Count);
    }

    [Fact]
    public void WorkSchedule_Period_Check_Day()
    {
        var period = new TimeShiftPeriod("Test", DateTime.Parse("01.04.2024"), DateTime.Parse("14.04.2024"), false);
        List<DateTime> days = period.GetAllDaysInPeriod(true).ToList();
        Assert.Equal(DateTime.Parse("03.04.2024"), days[2]);
    }

    [Fact]
    public void Should_Create_EmployeeName()
    {
        var name = "Иванов Иван Иванович";
        var empName = new EmployeeName(name);
        
        Assert.NotNull(empName);
        Assert.Equal("Иванов Иван Иванович", empName.FullName);
        Assert.Equal("Иван", empName.FirstName);
    }
    
    
    [Fact]
    public void Should_Create_Employee()
    {
        var staff = TestStaff();
        var name = new EmployeeName("Иванов Иван Иванович");
        var emp = new Employee(name, TestOrganization, TestDep(), staff);
        
        Assert.NotNull(emp);
        Assert.Equal("Иванов Иван Иванович", emp.Name.FullName);
        Assert.Equal(TestOrganization, emp.Organization);
    }

    [Fact]
    public void Should_Create_TimeShiftPeriod()
    {
        var period = new TimeShiftPeriod("Test", DateTime.Parse("01.04.2024"), DateTime.Parse("14.04.2024"), false);
        
        Assert.NotNull(period);
        Assert.Equal(DateTime.Parse("01.04.2024"), period.Start);
        Assert.Equal(DateTime.Parse("04.04.2024"), period.GetAllDaysInPeriod().ToList()[3]);
    }

    [Fact]
    public void Should_Create_TimeShift()
    {
        var name = new EmployeeName("Иванов Иван Иванович");
        var emp = new Employee(name, TestOrganization, TestDep(), TestStaff());
        emp.WorkSchedule = TestStaff().WorkSchedule;
        emp.TypeEmployment = new TypeOfWorkingTime("РВ");
        
        var period = new TimeShiftPeriod("Test", DateTime.Parse("01.04.2024"), DateTime.Parse("14.04.2024"), false);

        List<TimeShift> timeShifts = new List<TimeShift>();
        
        foreach (var day in period.GetAllDaysInPeriod(true).ToList())
        {
            timeShifts.Add(new TimeShift(period,emp, day));
        }
        
        Assert.NotNull(timeShifts);
        Assert.Equal(14, timeShifts.Count);
        Assert.Equal(8f, timeShifts[2].HoursPlanned);
        
    }
    
}