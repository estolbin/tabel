using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using web_tabel.Domain;
using web_tabel.Services;
using web_tabel.Services.TimeShiftContext;

namespace web_table.Web.Services
{
    public static class SeedData
    {
        private static async Task<Role> CheckCreateRole(TimeShiftDBContext context, string name, string description)
        {
            var role = await context.Roles.SingleOrDefaultAsync(r => r.Name == name);
            if (role == null)
            {
                role = new Role
                {
                    Name = name,
                    Description = description
                };
                await context.Roles.AddAsync(role);
                await context.SaveChangesAsync();
            }
            return role;
        }

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using(var context = new TimeShiftDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<TimeShiftDBContext>>(),
                serviceProvider.GetRequiredService<IHttpContextAccessor>()))
            {
                Role role = await CheckCreateRole(context, Constants.ADMNIM_ROLE, "Администратор");
                
                if (!context.Users.Any(u => u.Role == role))
                {
                    var admin = new AppUser
                    {
                        Name = "admin",
                        Password = "admin",
                        Email = "admin@admin",
                        Role = role,
                        RoleName = Constants.ADMNIM_ROLE
                    };
                    try
                    {

                    await context.Users.AddAsync(admin);
                    await context.SaveChangesAsync();
                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                _ = await CheckCreateRole(context, Constants.PAYROLL_ROLE, "Расчетный отдел");
                _ = await CheckCreateRole(context, Constants.TIMEKEEPER_ROLE, "Табельщик (прив.)");
                _ = await CheckCreateRole(context, Constants.USER_ROLE, "Табельщик");
            }
        }
    }
}
