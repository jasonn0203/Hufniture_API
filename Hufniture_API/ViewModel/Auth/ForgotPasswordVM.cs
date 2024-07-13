using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.ViewModel.Auth
{
    public class ForgotPasswordVM
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email không được để trống")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password không được để trống")]
        public required string NewPassword { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu mới không được để trống")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu và xác nhận mật khẩu mới không khớp")]
        public required string ConfirmNewPassword { get; set; }
    }
}
