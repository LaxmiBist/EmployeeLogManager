using EmployeeLogManager.DAL.Repositories;
using EmployeeLogManager.Model.Entities;
using EmployeeLogManager.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeLogManager.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<UserViewModel>> GetActiveUsersAsync()
        {
            var users = await _repo.GetActiveUsersAsync();
            return users.Select(u => new UserViewModel
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Address = u.Address,
                Department = u.Department,
                Position = u.Position
            }).ToList();
        }

        public async Task<List<Role>> GetRolesAsync() => await _repo.GetRolesAsync();

        public async Task<UserViewModel?> GetUserByIdAsync(int id)
        {
            var user = await _repo.GetUserByIdAsync(id);
            if (user == null) return null;

            return new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Department = user.Department,
                Position = user.Position
            };
        }

        public async Task<(bool Success, string Message)> CreateUserAsync(UserViewModel model)
        {
            try
            {
                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Department = model.Department,
                    Position = model.Position,
                    RoleId = model.RoleId
                };

                bool success = await _repo.CreateUserAsync(user);
                return (success, success ? "User created successfully!" : "Failed to create user.");
            }
            catch (DbUpdateException ex)
            {
                return (false, ex.InnerException?.Message.Contains("UNIQUE") ?? false 
                    ? "Email already exists." 
                    : "An error occurred while saving.");
            }
        }

        public async Task<(bool Success, string Message)> UpdateUserAsync(UserViewModel model)
        {
            var user = await _repo.GetUserByIdAsync(model.Id);
            if (user == null) return (false, "User not found.");

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Password = model.Password;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.Department = model.Department;
            user.Position = model.Position;
            user.UpdatedAt = DateTime.Now;

            bool success = await _repo.UpdateUserAsync(user);
            return (success, success ? "User updated successfully!" : "Failed to update user.");
        }

        public async Task<(bool Success, string Message)> SoftDeleteUserAsync(int id)
        {
            bool success = await _repo.SoftDeleteUserAsync(id);
            return (success, success ? "User deactivated successfully!" : "User not found.");
        }
    }
}