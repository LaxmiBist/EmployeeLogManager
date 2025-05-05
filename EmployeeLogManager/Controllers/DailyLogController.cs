using EmployeeLogManager.BLL.Services;
using EmployeeLogManager.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeLogManager.Controllers
{
    [Authorize(Roles = "User")]
    [Route("dailylog")]
    public class DailyLogController : Controller
    {
        private readonly IDailyLogService _service;

        public DailyLogController(IDailyLogService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized("User not authenticated.");

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
                return BadRequest("Invalid user ID.");

            var logs = await _service.GetUserLogsAsync(userId);
            return View(logs);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new DailyLogViewModel());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(DailyLogViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid data submitted.";
                return View(vm);
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                ViewBag.Error = "User not authenticated.";
                return View(vm);
            }

            try
            {
                bool success = await _service.CreateLogAsync(vm, userId);
                if (success)
                {
                    ViewBag.SuccessMsg = "Log submitted successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Failed to save log.";
                    return View(vm);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error: {ex.Message}";
                return View(vm);
            }
        }


        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var log = await _service.GetDailyLogDetailsAsync(id);
            if (log == null)
            {
                return NotFound();  
            }
            return View(log);  // Pass the view model to the details page
        }
    }
}