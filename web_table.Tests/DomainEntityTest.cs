using web_tabel.Domain;

namespace web_table.Tests;

public class DomainEntityTest
{
    Organization TestOrganization = new("Test");

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
        var typeEmployment = new TypeEmployment();
        typeEmployment.Name = "РB"; // рабочее время
        typeEmployment.Description = "Рабочее время";

        var typeEmployment1 = new TypeEmployment();
        typeEmployment1.Name = "РB"; // рабочее время
        typeEmployment1.Description = "Еще одно рабочее время";
        
        
        Assert.NotNull(typeEmployment);
        Assert.Equal(typeEmployment, typeEmployment1); // сравнение по наименованию
    }

    [Fact]
    public void Should_Exeption_When_Name_Is_Null()
    {
        var typeEmployment = new TypeEmployment();
        Assert.Throws<ArgumentNullException>(() => typeEmployment.Name = null);
    }

    [Fact]
    public void Should_Exeption_When_Name_More_Than_Max_Length()
    {
        var typeEmployment = new TypeEmployment();
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
        testStaff.SetNumberOfPositions(10.0f);

        Assert.Equal(10.0f, testStaff.NumberOfPositions);
    }
}