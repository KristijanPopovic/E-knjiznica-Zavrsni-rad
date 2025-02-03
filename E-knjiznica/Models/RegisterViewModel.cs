using System.ComponentModel.DataAnnotations;

namespace E_knjiznica.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email je obavezan.")]
        [EmailAddress(ErrorMessage = "Neispravna email adresa.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
