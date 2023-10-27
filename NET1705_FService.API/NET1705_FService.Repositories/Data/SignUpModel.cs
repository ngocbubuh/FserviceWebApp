using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace NET1705_FService.Repositories.Data
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Name is required!")]
        [Display(Name = "Name")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Phone number is required!")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number!")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number!")]
        [Display(Name = "Phone Number")]
        public required string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Address is required!")]
        [Display(Name = "Address")]
        public required string Address { get; set; }
        [Required(ErrorMessage = "Date of Birth is required!")]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Email is required!"), EmailAddress(ErrorMessage = "Must be email format!")]
        [Display(Name = "Email Address")]
        public required string Email { get; set; }
        //[Required, EmailAddress]
        //public string UserName { get; set; } = null!;
        [Required(ErrorMessage = "Password is required!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Password must be 7-12 Character")]
        [PasswordPropertyText]
        public required string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required!")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation does not match!")]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Password must be 7-12 Character")]
        [PasswordPropertyText]
        public required string ConfirmPassword { get; set; }
    }
}
