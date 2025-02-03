using Microsoft.AspNetCore.Identity;

namespace E_knjiznica.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Role { get; set; }  // ✅ Dodatno polje za ulogu (Admin/User)
    }
}
