using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeLogManager.Model.ViewModel
{
    public class DailyLogViewModel

    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [StringLength(100, ErrorMessage = "Department can't exceed 100 characters")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Project Name is required")]
        [StringLength(100, ErrorMessage = "Project name can't exceed 100 characters")]
        public string ProjectName { get; set; }

        [MinLength(1, ErrorMessage = "At least one task must be added")]
        public List<DailyLogTaskViewModel> Tasks { get; set; } = new();
    }


    public class DailyLogTaskViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Task is required")]
        [StringLength(255, ErrorMessage = "Task can't exceed 255 characters")]
        public string Task { get; set; }

        [StringLength(1000, ErrorMessage = "Notes can't exceed 1000 characters")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(50, ErrorMessage = "Status can't exceed 50 characters")]
        public string Status { get; set; }
    }

}
