using EmployeeLogManager.DAL.Repositories;
using EmployeeLogManager.Model.Entities;
using EmployeeLogManager.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeLogManager.BLL.Services
{
    public class DailyLogService : IDailyLogService
    {
        // (DI) service needs a data helper (repo), so DI gives it one.
        private readonly IDailyLogRepository _repo;

        public DailyLogService(IDailyLogRepository repo)
        {
            _repo = repo; // Store the injected dependency for later use.
        }


        // returns a collection (list of item)
        //List<T> Data is transformed, sorted, indexed, or updated.
        public async Task<List<DailyLogViewModel>> GetUserLogsAsync(int userId)
        {
            var logs = await _repo.GetLogsByUserIdAsync(userId);
            return logs.Select(log => new DailyLogViewModel
            {
                Id = log.Id,
                Date = log.Date,
                Department = log.Department,
                ProjectName = log.ProjectName,
                Tasks = log.Tasks.Select(t => new DailyLogTaskViewModel
                {
                    Task = t.Task,
                    Notes = t.Notes,
                    Status = t.Status
                }).ToList()
            }).ToList();
        }


        // returns a success flag (true or false).
        public async Task<bool> CreateLogAsync(DailyLogViewModel vm, int userId)
        {
            var log = new DailyLog
            {
                Date = vm.Date,
                Department = vm.Department,
                ProjectName = vm.ProjectName,
                CreatedAt = DateTime.Now,
                UserId = userId,
                Tasks = vm.Tasks.Select(t => new DailyLogTask
                {
                    Task = t.Task,
                    Notes = t.Notes,
                    Status = t.Status
                }).ToList()
            };

            return await _repo.CreateDailyLogAsync(log);
        }

        // In DailyLogService.cs
        //this method can return null too(?)
        public async Task<DailyLogViewModel?> GetDailyLogDetailsAsync(int id)
        {
            var log = await _repo.GetDailyLogByIdAsync(id);
            if (log == null) return null;

            return new DailyLogViewModel
            {
                Id = log.Id,
                Date = log.Date,
                Department = log.Department,
                ProjectName = log.ProjectName,
                Tasks = log.Tasks.Select(t => new DailyLogTaskViewModel
                {
                    Task = t.Task,
                    Notes = t.Notes,
                    Status = t.Status
                }).ToList()
            };
        }
    }
}