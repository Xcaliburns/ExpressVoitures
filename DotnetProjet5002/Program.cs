using DotnetProjet5.Data;
using DotnetProjet5.Models.Services;
using DotnetProjet5.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false) // Désactiver la confirmation de compte par email
    .AddRoles<IdentityRole>() // Add this line to include role management
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Add this line to include Razor Pages services
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IRepairService, RepairService>();
builder.Services.AddScoped<IFileUploadHelper, FileUploadHelper>();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

// Configurer la culture par défaut
var cultureInfo = new CultureInfo("en-EN");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    //Comment in development mode
    app.UseExceptionHandler("/Home/Error");
}
else
{   //Uncomment The next line  for development 
    // app.UseExceptionHandler("/Home/Error");

    //The next line is for production only comment it for development
    app.UseExceptionHandler("/Home/CustomError");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Middleware pour gérer les erreurs 404
app.UseStatusCodePagesWithReExecute("/Home/NotFound", "?statusCode={0}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    // Create default vehicle list and default admin user
    await SeedData.Initialize(services);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "roles",
    pattern: "roles",
    defaults: new { area = "Identity", controller = "Roles", action = "Index" });
app.MapAreaControllerRoute(
    name: "Identity",
    areaName: "Identity",
    pattern: "Identity/{controller=Roles}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "roles",
    pattern: "roles/{action=Index}/{id?}",
    defaults: new { area = "Identity", controller = "Roles" });
app.MapRazorPages(); // Ensure this line is present to map Razor Pages

app.Run();