using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_knjiznica.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; } // Primarni ključ recenzije

        [Required]
        public int BookId { get; set; } // ID knjige kojoj pripada recenzija
        public Book Book { get; set; } // Navigacijska svojina

        [Required]
        [StringLength(50)]
        public string UserName { get; set; } // Ime korisnika koji je ostavio recenziju

        [Required]
        [Range(1, 10)]
        public int Rating { get; set; } // Ocjena (1-10)
    }
}
