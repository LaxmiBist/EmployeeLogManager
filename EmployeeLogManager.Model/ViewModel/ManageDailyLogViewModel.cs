
namespace EmployeeLogManager.Model.ViewModel
{
    public class ManageDailyLogViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }   
        public string Department { get; set; }
        public string Project { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public List<DailyLogTaskViewModel> Tasks { get; set; } = new();
    }

    }

