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

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite(configuration.GetConnectionString("DevelopConnection"));
        }
        else
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Employee>().OwnsOne(e => e.Name);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Organization)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.StaffSchedule)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<WorkSchedule>()
            .OwnsMany(w => w.HoursByDayNumbers, h =>
            {
                h.WithOwner().HasForeignKey("WorkScheduleId");
                h.HasKey(x => new { x.WorkScheduleId, x.DayNumber, x.TypeOfWorkingTimeName });
            });

        modelBuilder.Entity<TypeOfWorkingTime>()
            .HasKey(t => new { t.Name });
        
        modelBuilder.Entity<TypeOfEmployment>()
            .HasKey(t => new {t.Name});

        modelBuilder.Entity<WorkingCalendar>()
            .HasKey(t => new { t.Date });

        modelBuilder.Entity<WorkingCalendar>()
            .HasIndex(c => new { c.Year }, "IX_WorkingCalendar_Year");


        modelBuilder.Entity<EmployeeCondition>()
            .HasKey(c => new { c.Name });

        modelBuilder.Entity<StaffSchedule>()
            .HasOne(s => s.Organization)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<TimeShift>()
            .HasOne(t => t.WorkSchedule)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        base.OnModelCreating(modelBuilder);

    }
}