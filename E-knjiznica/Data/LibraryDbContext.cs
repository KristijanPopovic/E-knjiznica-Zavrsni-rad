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
        public Author Author { get; set; }      // ✅ Dodano za relaciju s autorom
        public IdentityUser User { get; set; }  // ✅ Dodano za relaciju s korisnikom
        public DbSet<FavoriteAuthor> FavoriteAuthors { get; set; } // ✅ Dodano za omiljene autore

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Relacija između Book i User (posuđene knjige)
            modelBuilder.Entity<Book>()
                .HasOne<IdentityUser>(b => b.BorrowedByUser)
                .WithMany()
                .HasForeignKey(b => b.BorrowedByUserId);

            // ✅ Relacija između Author i Book (jedan autor - više knjiga)
            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId);

            // ✅ Relacija između FavoriteAuthor i User (omiljeni autori)
            modelBuilder.Entity<FavoriteAuthor>()
                .HasOne(fa => fa.User)
                .WithMany()
                .HasForeignKey(fa => fa.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Kada se korisnik obriše, brišu se i njegovi omiljeni autori

            // ✅ Relacija između FavoriteAuthor i Author
            modelBuilder.Entity<FavoriteAuthor>()
                .HasOne(fa => fa.Author)
                .WithMany()
                .HasForeignKey(fa => fa.AuthorId)
                .OnDelete(DeleteBehavior.Cascade); // Kada se autor obriše, brišu se i svi njegovi zapisi u omiljenim autorima
        }
    }
}
