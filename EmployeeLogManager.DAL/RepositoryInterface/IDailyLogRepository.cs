using EmployeeLogManager.Model.Entities;


namespace EmployeeLogManager.DAL.Repositories
{
    public interface IDailyLogRepository
    {
        Task<IEnumerable<DailyLog>> GetLogsByUserIdAsync(int userId);
        Task<bool> CreateDailyLogAsync(DailyLog log);
        Task<DailyLog?> GetDailyLogByIdAsync(int id);  // Returns nullable in case log doesn't exist
    }
}