using EmployeeLogManager.DAL.Data;
using EmployeeLogManager.Model.Entities;
using Microsoft.EntityFrameworkCore;

public class RoleRepository : IRoleRepository
{
    private readonly EmployeeLogManagerDbcontext _db;

    public RoleRepository(EmployeeLogManagerDbcontext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        return await _db.Roles.ToListAsync();
    }

    public async Task<Role> GetByIdAsync(int id)
    {
        return await _db.Roles.FindAsync(id);
    }

    public async Task<Role> GetByNameAsync(string name)
    {
        return await _db.Roles.FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<bool> CreateAsync(Role role)
    {
        await _db.Roles.AddAsync(role);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Role role)
    {
        _db.Roles.Update(role);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var role = await _db.Roles.FindAsync(id);
        if (role == null) return false;
        _db.Roles.Remove(role);
        return await _db.SaveChangesAsync() > 0;
    }
}
