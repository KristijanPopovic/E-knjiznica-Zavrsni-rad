using Microsoft.AspNetCore.Identity;

namespace E_knjiznica.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }  // Admin ili User
    }
}
