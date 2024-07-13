using Hufniture_API.Enums;
using Microsoft.AspNetCore.Identity;

namespace Hufniture_API.Data
{
    public class AppUser : IdentityUser
    {
        public string? Address { get; set; }
        public required string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
    }
}
