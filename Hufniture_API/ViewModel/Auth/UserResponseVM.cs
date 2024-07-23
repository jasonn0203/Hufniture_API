using Hufniture_API.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hufniture_API.ViewModel.Auth
{
    public class UserResponseVM
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email không được để trống")]
        public required string Email { get; set; }

        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public required string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }

        public string Id { get; set;}
    }
}
