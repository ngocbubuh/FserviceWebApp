using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NET1705_FService.Repositories.Data
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Username is required!"), EmailAddress(ErrorMessage = "Must be email address format!")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        [PasswordPropertyText]
        public required string Password { get; set; }
    }
}
