using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.ViewModel.Auth
{
    public class CheckEmailVM
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; }
    }

}
