using Microsoft.EntityFrameworkCore;
using web_tabel.Domain;

namespace web_tabel.Services.TimeShiftContext;

public class TimeShiftDBContext : DbContext
{
    public DbSet<TimeShiftPeriod> TimeShiftPeriods { get; set; }
    public DbSet<TimeShift> TimeShifts { get; set; }
    public DbSet<WorkSchedule> WorkSchedules { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeName> EmployeeNames { get; set; }
    public DbSet<TypeEmployment> TypeEmployments { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<StaffSchedule> StaffSchedules { get; set; }
    
    public TimeShiftDBContext(DbContextOptions<TimeShiftDBContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TypeEmployment>()
            .HasKey(t => new { t.Name });
    }
}