using EmployeeLogManager.Model.Entities;
using EmployeeLogManager.Model.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeLogManager.BLL.Services
{
    public interface IDailyLogService
    {
        Task<List<DailyLogViewModel>> GetUserLogsAsync(int userId);
        Task<bool> CreateLogAsync(DailyLogViewModel vm, int userId);
       
        Task<DailyLogViewModel?> GetDailyLogDetailsAsync(int id);
    }
}