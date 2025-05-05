using Azure;
using EmployeeLogManager.DAL.Data;
using EmployeeLogManager.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeLogManager.DAL.Repositories
{
    public class DailyLogRepository : IDailyLogRepository
    {
        private readonly EmployeeLogManagerDbcontext _context;

        public DailyLogRepository(EmployeeLogManagerDbcontext context)
        {
            _context = context;
        }

        //	IEnumerable<T>	Just fetching data. No need for indexing or modifications.
        public async Task<IEnumerable<DailyLog>> GetLogsByUserIdAsync(int userId)
        {
            return await _context.DailyLogs
                .Include(dl => dl.Tasks)  // Eagerly load the Orders related to this Customer
                .Where(dl => dl.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> CreateDailyLogAsync(DailyLog log)
        {
            await _context.DailyLogs.AddAsync(log);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<DailyLog?> GetDailyLogByIdAsync(int id)
        {
            return await _context.DailyLogs
                .Include(dl => dl.Tasks)  // Include related Tasks
                .FirstOrDefaultAsync(dl => dl.Id == id);
        }
    }
}


//Note:
//eager loading means that related data is loaded at the same time
//as the main entity in a single query. This is done by using the 
//Include method in Entity Framework.