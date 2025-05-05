using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeLogManager.Model.Entities
{
    public class DailyLog : BaseEntity
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(100)]
        public string Department { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProjectName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<DailyLogTask> Tasks { get; set; } = new(); // auto-initialized(new empty list when "Dailylog" obj created {var log = new DailyLog();

        //Navigation Property
        [ForeignKey("UserId")]
        public virtual User Users { get; set; }
    }

    public class DailyLogTask
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Task { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        [ForeignKey("DailyLog")]
        public int DailyLogId { get; set; }

        public DailyLog DailyLog
        {
            get; set;

        }

    }
}
