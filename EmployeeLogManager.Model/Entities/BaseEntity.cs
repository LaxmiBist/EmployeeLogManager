
using System.ComponentModel.DataAnnotations;


namespace EmployeeLogManager.Model.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // current date and time in UTC (Coordinated Universal Time) not local time.

        public DateTime? UpdatedAt { get; set; }
    }
}


//Note : Use abstract when the class is meant only to be inherited and not used directly.