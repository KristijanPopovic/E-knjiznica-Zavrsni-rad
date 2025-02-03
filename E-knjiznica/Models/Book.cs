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
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Genre { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BorrowedDate { get; set; } = null;

        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; } = null;

        public bool IsBorrowed { get; set; } = false;

        [ForeignKey("BorrowedByUser")]
        public string? BorrowedByUserId { get; set; }
        public IdentityUser? BorrowedByUser { get; set; }

        [ForeignKey("Author")]
        public int? AuthorId { get; set; }
        public Author? Author { get; set; }

        public List<Review> Reviews { get; set; } = new List<Review>();

        public double AverageRating => (Reviews != null && Reviews.Any()) ? Reviews.Average(r => r.Rating) : 0;

        public string PublishedYear { get; set; }
        public string CoverUrl { get; set; }
    }
}
