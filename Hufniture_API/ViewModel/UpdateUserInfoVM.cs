using Hufniture_API.Enums;

namespace Hufniture_API.ViewModel
{
    public class UpdateUserInfoVM
    {
        public string? Address { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
    }
}
