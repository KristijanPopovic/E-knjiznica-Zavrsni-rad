using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_knjiznica.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string Author { get; set; }

        [Required]
        public string Genre { get; set; }

        public string? Review { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BorrowedDate { get; set; } // Kada je posuđena knjiga

        public bool IsBorrowed { get; set; } = false; // Da li je knjiga trenutno posuđena

        [ForeignKey("User")]
        public int? BorrowedByUserId { get; set; } // ID korisnika koji je posudio knjigu
        public User? BorrowedByUser { get; set; } // Navigacijska svojnost
    }
}
