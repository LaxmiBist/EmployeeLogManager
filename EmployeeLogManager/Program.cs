using EmployeeLogManager.BLL.ServiceInterfaces;
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

            // This tells the app to use SQL Server and get the connection string from the settings using EF Core.
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
            builder.Services.AddScoped<IJobTestService, JobTestService>();
            //builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
            builder.Services.AddScoped<DailyLogReminderService>();




            // Register and configure Hangfire service with dependency injection
            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer() // Use simple names for job type serialization (easier to read and debug)
                .UseRecommendedSerializerSettings() // Use recommended default settings for the JSON serializer
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("SqlServerConnection"))  // Tell Hangfire to use SQL Server for job storage (connection string from appsettings.json)
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
           /* app.UseHangfireDashboard("/custompath", new DashboardOptions
            {
                DarkModeEnabled = false,
                DisplayStorageConnectionString = false,
                Authorization = new[]
                {
                 new HangfireCustomBasicAuthenticationFilter
                {
                  User = "admin",
                  Pass = "admin123"
                }
                }
            });*/



            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            using (var scope = app.Services.CreateScope()) // create a scope
            {
                var jobService = scope.ServiceProvider.GetRequiredService<IJobTestService>(); // get job service
                jobService.ScheduleJobs(); // run job scheduler
            }



            app.Run();
        }
    }
}
