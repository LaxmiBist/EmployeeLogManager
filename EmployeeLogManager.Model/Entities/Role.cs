using System.ComponentModel.DataAnnotations;

namespace EmployeeLogManager.Model.Entities
{
    public class Role:BaseEntity
    {
        [Required(ErrorMessage = "Role name is required")]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }

}
