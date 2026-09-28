using Microsoft.AspNetCore.Identity;

namespace BusBookingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
