using EmployeeLogManager.Model.Entities;

public interface IRoleRepository
{
    Task<Role> GetByIdAsync(int id);
    Task<IEnumerable<Role>> GetAllAsync();
    Task<Role> GetByNameAsync(string name);
    Task<bool> CreateAsync(Role role);
    Task<bool> UpdateAsync(Role role);
    Task<bool> DeleteAsync(int id);
}
