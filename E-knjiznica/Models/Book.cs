using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace E_knjiznica.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; } // Primarni ključ knjige

        [Required]
        [StringLength(100)]
        public string Title { get; set; } // Naslov knjige

        [Required]
        [StringLength(50)]
        public string Author { get; set; } // Autor knjige

        [Required]
        public string Genre { get; set; } // Žanr knjige

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; } // Datum izdavanja knjige

        [DataType(DataType.Date)]
        public DateTime? BorrowedDate { get; set; } // Datum kada je knjiga posuđena (može biti null ako nije posuđena)

        public bool IsBorrowed { get; set; } = false; // Oznaka je li knjiga trenutno posuđena

        
        [ForeignKey("BorrowedByUser")]
        public string? BorrowedByUserId { get; set; }  // ✅ OVO JE ISPRAVNO
                                                       // ID korisnika koji je posudio knjigu

        public IdentityUser? BorrowedByUser { get; set; }


        public List<Review> Reviews { get; set; } = new List<Review>(); // Lista recenzija knjige

        // Automatski izračun prosječne ocjene knjige (ako nema recenzija, vraća 0)
        public double AverageRating => (Reviews != null && Reviews.Any()) ? Reviews.Average(r => r.Rating) : 0;
    }
}
