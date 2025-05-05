using EmployeeLogManager.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmployeeLogManager.DAL.Data
{
    public class EmployeeLogManagerDbcontext : DbContext
    {
        public EmployeeLogManagerDbcontext(DbContextOptions<EmployeeLogManagerDbcontext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<DailyLog> DailyLogs { get; set; }

        public DbSet<DailyLogTask> DailyLogTasks { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            );

            // Seed Admin User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "Admin User",
                    Email = "admin@company.com",
                    Password = "admin123",
                    PhoneNumber = "1234567890",
                    Address = "Admin Office",
                    Department = "Management",
                    Position = "Administrator",
                    IsActive = true,
                    RoleId = 1
                }
            );
        }
    }
}


/* Note:
- OnModelCreating runs during migration/database creation.
- It uses ModelBuilder to define: 1.How your entities behave, 2.What data should be added (seeding).

- HasData(...) is used to insert that data during the migration.*/