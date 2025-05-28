using Microsoft.AspNetCore.Mvc;
using EmployeeLogManager.BLL.ServiceInterfaces;

namespace EmployeeLogManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTestController : ControllerBase
    {
        private readonly IJobTestService _jobTestService;

        public JobTestController(IJobTestService jobTestService)
        {
            _jobTestService = jobTestService;
        }

        [HttpPost("schedule")]
        public IActionResult ScheduleJob()
        {
            _jobTestService.ScheduleJobs();
            return Ok("Recurring job scheduled successfully.");
        }
    }
}
