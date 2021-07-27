using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Empty field.")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Wrong email.")]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required(ErrorMessage ="This field required password.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and the confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        [Required(ErrorMessage ="Please, choose role")]
        [Display(Name = "Role name")]
        public string RoleName { get; set; }
    }
}
