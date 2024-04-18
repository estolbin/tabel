using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using web_tabel.Domain;
using web_tabel.Services;
using web_tabel.Services.TimeShiftContext;
using web_table.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TimeShiftDBContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// seed data
builder.Services.AddScoped<DbInitializer>();


builder.Services.AddScoped<ITimeShiftService, TimeShiftService>();
builder.Services.AddScoped<ITimeShiftRepository, TimeShiftRepository>();

var app = builder.Build();

app.UseItToSeedSqliteServer();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
