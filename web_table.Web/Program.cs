using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using web_tabel.Domain;
using web_tabel.Services;
using web_tabel.Services.TimeShiftContext;
using web_table.Web;
using web_table.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<TimeShiftDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        
});

// authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = Constants.AUTH_COOKIE_NAME;
        options.LoginPath = new PathString("/Account/Login");
        options.AccessDeniedPath = new PathString("/Error/AccessDenied");

    });

// policy for admin
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.Requirements.Add(new RoleRequirement(Constants.ADMNIM_ROLE)));
});

builder.Services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();

builder.Services.AddScoped<CurrentUserProvider>();
builder.Services.AddScoped<UserService>();
builder.Services.AddHttpContextAccessor();


// seed data
builder.Services.AddScoped<DbInitializer>();


builder.Services.AddScoped<ITimeShiftService, TimeShiftService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".web_table.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
});
builder.Services.AddApplicationInsightsTelemetry();


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
app.UseSession();

app.UseMiddleware<AuthenticationMiddleware>();
app.UseMiddleware<AuthorizationErrorMiddleware>();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TimeShift}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

app.Run();
