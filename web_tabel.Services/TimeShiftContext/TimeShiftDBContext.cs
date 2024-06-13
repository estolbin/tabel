using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using web_tabel.Domain;
using web_tabel.Domain.UserFilters;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace web_tabel.Services.TimeShiftContext;

public class TimeShiftDBContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

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
    public DbSet<EmployeeCondition> EmployeeCondition { get; set; }
    public DbSet<EmployeeState> EmployeeStates { get; set; }

    public DbSet<TypeOfWorkingTimeRules> TypeOfWorkingTimeRules { get; set; } 
    
    public DbSet<AppUser> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Filter> Filters { get; set; }


    public TimeShiftDBContext(
        DbContextOptions<TimeShiftDBContext> options, 
        IHttpContextAccessor httpContextAccessor) : base(options)
    {
        this.ChangeTracker.LazyLoadingEnabled = true;
        _httpContextAccessor = httpContextAccessor;
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
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
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
            .HasKey(c => c.Name);

        modelBuilder.Entity<EmployeeState>()
            .HasOne(s => s.Employee)
            .WithMany() 
            .HasForeignKey("EmployeeId");

        modelBuilder.Entity<EmployeeState>()
            .HasOne(s => s.EmployeeCondition)
            .WithMany()
            .HasForeignKey("ConditionName")
            .HasPrincipalKey(e => e.Name);

        modelBuilder.Entity<StaffSchedule>()
            .HasOne(s => s.Organization)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<TimeShift>()
            .HasOne(t => t.WorkSchedule)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<TypeOfWorkingTimeRules>()
            .HasKey(t => new { t.SourceName, t.TargetName });

        modelBuilder.Entity<TypeOfWorkingTimeRules>()
            .HasOne(t => t.Source)
            .WithMany()
            .HasForeignKey(t => t.SourceName)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<TypeOfWorkingTimeRules>()
            .HasOne(t => t.Target)
            .WithMany()
            .HasForeignKey(t => t.TargetName)
            .OnDelete(DeleteBehavior.NoAction);


        // users and filters
        modelBuilder.Entity<AppUser>()
            .HasOne(u => u.Filter)
            .WithMany()
            .HasForeignKey(u => u.FilterId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Filter>()
            .HasDiscriminator<string>("FilterType")
            .HasValue<OrganizationFilter>("Organization")
            .HasValue<DepartmentFilter>("Department")
            .HasValue<CompositeFilter>("Composite");

        modelBuilder.Entity<Filter>()
            .Property(f => f.FilterType)
            .HasMaxLength(50);

        modelBuilder.Entity<OrganizationFilter>()
            .HasMany(o => o.Organizations)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DepartmentFilter>()
            .HasMany(d => d.Departemnts)
            .WithOne() 
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CompositeFilter>()
            .HasMany(c => c.Organizations)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CompositeFilter>()
            .HasMany(c => c.Departments)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);


        // filters
        //modelBuilder.Entity<Department>()
        //    .HasQueryFilter(d =>
        //        EF.Property<Guid>(d, "Id") == GetUserDepsId() || IsUserAdmin());


        base.OnModelCreating(modelBuilder);

    }

    //private Guid GetUserDepsId()
    //{
    //    var userInClaim = _httpContextAccessor.HttpContext.User.Claims
    //        .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
    //    if(int.TryParse(userInClaim.Value, out int userId))
    //    {
    //        var user = Users.FirstOrDefault(u => u.Id == userId);
    //        if (user.Filter != null)
    //        {
    //            if(user.Filter.FilterType == "Department")
    //            {
    //                var filterDep = user.Filter as DepartmentFilter;
    //                if(filterDep != null)
    //                {
    //                    return filterDep.DepartmentIds.FirstOrDefault();
    //                }
    //            }
    //        }
    //    }
    //    return Guid.Empty;
    //}

    //private bool IsUserAdmin()
    //{
    //    return _httpContextAccessor.HttpContext.User.IsInRole(Constants.ADMNIM_ROLE);
    //}
}