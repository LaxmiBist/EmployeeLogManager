using EmployeeLogManager.DAL.Data;
using EmployeeLogManager.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLogManager.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EmployeeLogManagerDbcontext _context;

        public UserRepository(EmployeeLogManagerDbcontext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetActiveUsersAsync()
        {
            return await _context.Users
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.IsActive = false;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}