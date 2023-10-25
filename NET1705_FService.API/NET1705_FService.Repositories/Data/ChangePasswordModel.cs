using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Data
{
    public class ChangePasswordModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Must be email formated!")]
        public string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Password must be 7-12 Character")]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Password must be 7-12 Character")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Password must be 7-12 Character")]
        [Display(Name = "Confirm new Password")]
        [Compare("NewPassword", ErrorMessage = "New password and confirmation does not match!")]
        public string ConfirmNewPassword { get; set;}
    }
}
