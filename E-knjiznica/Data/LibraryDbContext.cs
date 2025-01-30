using Microsoft.EntityFrameworkCore;
using E_knjiznica.Models;

namespace E_knjiznica.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Definiramo odnos knjiga-korisnici (jedan korisnik može posuditi više knjiga)
            modelBuilder.Entity<User>()
                .HasMany(u => u.BorrowedBooks)
                .WithOne(b => b.BorrowedByUser)
                .HasForeignKey(b => b.BorrowedByUserId);
        }
    }
}
