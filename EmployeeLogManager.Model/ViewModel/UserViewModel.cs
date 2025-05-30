﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeLogManager.Model.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address can't exceed 200 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Phone number must contain only digits (10 to 15 digits).")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Department is required.")]
        [StringLength(50, ErrorMessage = "Department can't exceed 50 characters.")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Position is required.")]
        [StringLength(50, ErrorMessage = "Position can't exceed 50 characters.")]
        public string Position { get; set; }

        public int RoleId { get; set; }
    }
}
