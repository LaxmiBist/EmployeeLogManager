using EmployeeLogManager.Model.Entities;


namespace EmployeeLogManager.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetActiveUsersAsync();
        Task<List<Role>> GetRolesAsync();
        Task<User?> GetUserByIdAsync(int id);

        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> SoftDeleteUserAsync(int id);
    }
}