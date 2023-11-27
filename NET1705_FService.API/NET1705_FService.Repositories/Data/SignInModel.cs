using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NET1705_FService.Repositories.Data
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Email không được để trống!"), EmailAddress(ErrorMessage = "Vui lòng nhập email hợp lệ!")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        [DataType(DataType.Password)]
        [PasswordPropertyText]
        public required string Password { get; set; }
    }
}
