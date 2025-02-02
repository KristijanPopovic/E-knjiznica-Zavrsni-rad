using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace E_knjiznica.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }  // Primarni ključ

        [Required]
        [StringLength(100)]
        public string Name { get; set; }  // Ime autora

        [DataType(DataType.MultilineText)]
        public string? Biography { get; set; }  // Biografija (opcionalno)

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }  // Datum rođenja (opcionalno)

        [DataType(DataType.Date)]
        public DateTime? DeathDate { get; set; }  // Datum smrti (opcionalno)

        public string? PhotoUrl { get; set; }  // URL slike autora (opcionalno)

        // ✅ Povezivanje autora s knjigama
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
