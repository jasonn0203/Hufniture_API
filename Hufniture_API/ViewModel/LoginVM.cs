using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.ViewModel
{
    public class LoginVM
    {

        [EmailAddress]
        [Required(ErrorMessage = "Email không được để trống")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password không được để trống")]
        public required string Password { get; set; }
    }
}
