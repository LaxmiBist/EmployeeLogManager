using Microsoft.EntityFrameworkCore;
using EmployeeLogManager.DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeLogManager.Model.ViewModel;

namespace EmployeeLogManager.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("ManageDailyLog")]
    public class ManageDailyLogController : Controller
    {
        private readonly EmployeeLogManagerDbcontext _context;

        public ManageDailyLogController(EmployeeLogManagerDbcontext context)
        {
            _context = context;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var logs = await _context.DailyLogs // Get all daily log records from the database
     .Include(log => log.Users) // Also include information about the user who created each log
     .Select(log => new ManageDailyLogViewModel // Convert each log into a simpler object (a view model) with only the needed info
     {
                        Id = log.Id,
                        Date = log.Date,
                        Department = log.Department,
                        Project = log.ProjectName,
                        UserName = log.Users.FullName,
                        Email = log.Users.Email
                    })
                    .ToListAsync();

                return View(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var log = await _context.DailyLogs // Get the daily logs from the database
      .Include(log => log.Users) // Also get user details linked to each log
      .Include(log => log.Tasks) // Also get the list of tasks related to each log( Eager Loading (Related Tables))
      .Where(log => log.Id == id) // Only take the log that matches the given ID(filtering record)
      .Select(log => new ManageDailyLogViewModel // Create a simpler object (ViewModel) with only the needed fields
      {
                        Id = log.Id,
                        Date = log.Date,
                        Department = log.Department,
                        Project = log.ProjectName,
                        UserName = log.Users.FullName,
                        Email = log.Users.Email,
                        Tasks = log.Tasks.Select(t => new DailyLogTaskViewModel
                        {
                            Task = t.Task,
                            Notes = t.Notes,
                            Status = t.Status
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();


                return View(log); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}


//Note
//Projection = Picking + reshaping only what you need (like SELECT name, email FROM Users in SQL).