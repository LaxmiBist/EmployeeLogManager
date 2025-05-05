using EmployeeLogManager.BLL.Services;
using EmployeeLogManager.DAL.Data;
using EmployeeLogManager.DAL.Repositories;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLogManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add database support using Entity Framework Core
            // This tells the app to use SQL Server and get the connection string from the settings
            builder.Services.AddDbContext<EmployeeLogManagerDbcontext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"))
            );

            // Set up authentication for the app using cookies
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                // Configure what happens when a user needs to log in or is denied access
                .AddCookie(options =>
                {
                    // If the user is not logged in, send them to this login page
                    options.LoginPath = "/Auth/Login";

                    // If the user is logged in but not allowed to access a page, send them here
                    options.AccessDeniedPath = "/Auth/AccessDenied";
                });

            //Registering class for DI(Instead of creating objects manually using new, you let .NET automatically create and inject them where needed.)

            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IDailyLogRepository, DailyLogRepository>();
            builder.Services.AddScoped<IDailyLogService, DailyLogService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            // Register and configure Hangfire service with dependency injection
            builder.Services.AddHangfire(configuration => configuration

                // Set compatibility level to 1.7/1.8 to maintain compatibility with previous Hangfire versions
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)

                // Use simple names for job type serialization (easier to read and debug)
                .UseSimpleAssemblyNameTypeSerializer()

                // Use recommended default settings for the JSON serializer
                .UseRecommendedSerializerSettings()

                // Tell Hangfire to use SQL Server for job storage (connection string from appsettings.json)
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("SqlServerConnection"))
            );

            // Register the Hangfire background job server that processes the jobs
            builder.Services.AddHangfireServer();



            var app = builder.Build();

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


            app.UseAuthentication(); // if using cookies
            app.UseAuthorization();

            // Optional: Enable Hangfire Dashboard
            app.UseHangfireDashboard();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
