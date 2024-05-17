using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using web_tabel.Domain;

namespace web_tabel.Services.TimeShiftContext;

public class TimeShiftDBContext : DbContext
{
    public DbSet<TimeShiftPeriod> TimeShiftPeriods { get; set; }
    public DbSet<TimeShift> TimeShifts { get; set; }
    public DbSet<WorkSchedule> WorkSchedules { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeName> EmployeeNames { get; set; }
    public DbSet<TypeOfWorkingTime> TypeEmployments { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<StaffSchedule> StaffSchedules { get; set; }

    public DbSet<TypeOfEmployment> TypeOfEmployments { get; set; }

    public TimeShiftDBContext(DbContextOptions<TimeShiftDBContext> options) : base(options)
    {
        this.ChangeTracker.LazyLoadingEnabled = true;
    }

    public TimeShiftDBContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .SetBasePath(Directory.GetCurrentDirectory())
            .Build();

        optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            //.UseSqlite("Data Source=TimeShift.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Employee>().OwnsOne(e => e.Name);

        modelBuilder.Entity<WorkSchedule>()
            .OwnsMany(w => w.HoursByDayNumbers, h =>
            {
                h.WithOwner().HasForeignKey("WorkScheduleId");
                h.HasKey(x => new { x.WorkScheduleId, x.DayNumber, x.TypeOfWorkingTimeName });
            });

        modelBuilder.Entity<TypeOfWorkingTime>()
            .HasKey(t => new { t.Name });
     
        
        base.OnModelCreating(modelBuilder);

    }
}