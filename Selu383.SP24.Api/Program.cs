using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Selu383.SP24.Api.Data;
using Selu383.SP24.Api.Features.Hotels;
using Selu383.SP24.Api.Features.Roles;
using Selu383.SP24.Api.Features.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataContext")));




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<DataContext>();
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
    options.Events.OnRedirectToLogin = (context) =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = (context) =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };

});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    await db.Database.MigrateAsync();

    var hotels = db.Set<Hotel>();

    if (!await hotels.AnyAsync())
    {
        for (int i = 0; i < 6; i++)
        {
            db.Set<Hotel>()
                .Add(new Hotel
                {
                    Name = "Hammond " + i,
                    Address = "1234 Place st"
                });
        }

        await db.SaveChangesAsync();
    }


    if (!await hotels.AnyAsync())
    {

        var roles = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        await roles.CreateAsync(new Role
        {
            Name = "Admin"
        }) ;

        await roles.CreateAsync(new Role
        {
            Name = "User"
        }) ;



        await db.SaveChangesAsync();
    }





}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0
// Hi 383 - this is added so we can test our web project automatically
public partial class Program { }
