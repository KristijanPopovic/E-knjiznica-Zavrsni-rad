using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E_knjiznica.Models;

namespace E_knjiznica.Data
{
    public class LibraryDbContext : IdentityDbContext<IdentityUser>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Author> Authors { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Ispravljena veza između knjiga i korisnika koristeći IdentityUser
            modelBuilder.Entity<Book>()
                .HasOne<IdentityUser>(b => b.BorrowedByUser)
                .WithMany()
                .HasForeignKey(b => b.BorrowedByUserId);
            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(b => b.Author)
    .HasForeignKey(b => b.AuthorId);

        }
    }
}
