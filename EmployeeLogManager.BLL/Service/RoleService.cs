using EmployeeLogManager.Model.Entities;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repo;

    public RoleService(IRoleRepository repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<Role>> GetAllAsync() => _repo.GetAllAsync();
    public Task<Role> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task<bool> CreateAsync(Role role) => _repo.CreateAsync(role);
    public Task<bool> UpdateAsync(Role role) => _repo.UpdateAsync(role);
    public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);
}
