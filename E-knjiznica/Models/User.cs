using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_knjiznica.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        public List<Book>? BorrowedBooks { get; set; } // Povijest posuđenih knjiga
    }
}
