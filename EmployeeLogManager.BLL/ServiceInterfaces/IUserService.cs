using EmployeeLogManager.Model.Entities;
using EmployeeLogManager.Model.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeLogManager.BLL.Services
{
    public interface IUserService
    {
        Task<List<UserViewModel>> GetActiveUsersAsync();
        Task<List<Role>> GetRolesAsync();
        Task<UserViewModel?> GetUserByIdAsync(int id);
        Task<(bool Success, string Message)> CreateUserAsync(UserViewModel model);
        Task<(bool Success, string Message)> UpdateUserAsync(UserViewModel model);
        Task<(bool Success, string Message)> SoftDeleteUserAsync(int id);
    }
}