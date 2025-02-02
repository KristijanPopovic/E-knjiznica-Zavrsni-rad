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
        public string Genre { get; set; } // Žanr knjige

        [Required]
        [DataType(DataType.Date)]
        public DateTime? BorrowedDate { get; set; } = new DateTime(1753, 1, 1); // Datum kada je knjiga posuđena

        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; } = new DateTime(1753, 1, 1); // Datum povrata

        public bool IsBorrowed { get; set; } = false; // Oznaka je li knjiga posuđena

        [ForeignKey("BorrowedByUser")]
        public string? BorrowedByUserId { get; set; } // ID korisnika koji je posudio knjigu
        public IdentityUser? BorrowedByUser { get; set; }

        // ✅ Dodano povezivanje s autorom
        [ForeignKey("Author")]
        public int? AuthorId { get; set; }
        public Author? Author { get; set; }

        public List<Review> Reviews { get; set; } = new List<Review>(); // Lista recenzija

        public double AverageRating => (Reviews != null && Reviews.Any()) ? Reviews.Average(r => r.Rating) : 0;

        public string PublishedYear { get; set; } // Godina izdavanja
        public string CoverUrl { get; set; } // Slika naslovnice
    }
}
