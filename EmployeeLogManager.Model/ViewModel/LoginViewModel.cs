using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace EmployeeLogManager.Model.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "UserName or Email is required.")]
        [MaxLength(50, ErrorMessage = "Max 20 Char allowed.")]
        [DisplayName("Username or Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Max 20 or min 5 character is allowed")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}



