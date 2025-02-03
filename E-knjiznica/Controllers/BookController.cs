using Microsoft.AspNetCore.Mvc;
using E_knjiznica.Data;
using E_knjiznica.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace E_knjiznica.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BooksController(LibraryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchTitle, string searchAuthor, string searchGenre, int? searchYear)
        {
            try
            {
                var books = _context.Books.Include(b => b.Author).AsQueryable();

                if (!string.IsNullOrEmpty(searchTitle))
                    books = books.Where(b => b.Title.Contains(searchTitle));

                if (!string.IsNullOrEmpty(searchAuthor))
                    books = books.Where(b => b.Author != null && b.Author.Name.Contains(searchAuthor));

                if (!string.IsNullOrEmpty(searchGenre))
                    books = books.Where(b => b.Genre.Contains(searchGenre));

                if (searchYear.HasValue)
                    books = books.Where(b => b.PublishedYear == searchYear.Value.ToString());

                var result = await books.ToListAsync();
                return View(result);
            }
            catch (Exception ex)
            {
                return Content($"❌ Greška prilikom učitavanja podataka: {ex.Message}\n\n📋 Detalji: {ex.StackTrace}");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Borrow(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null || book.IsBorrowed)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User); // ✅ Dohvaćanje ID-a korisnika

            book.IsBorrowed = true;
            book.BorrowedDate = DateTime.Now;
            book.ReturnDate = DateTime.Now.AddDays(14); // Posudba na 14 dana
            book.BorrowedByUserId = userId; // ✅ Postavljanje korisnika koji je posudio knjigu

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Return(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null || !book.IsBorrowed)
            {
                return NotFound();
            }

            book.IsBorrowed = false;
            book.BorrowedDate = null;
            book.ReturnDate = null;
            book.BorrowedByUserId = null;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SavedBooks()
        {
            var userId = _userManager.GetUserId(User);

            var books = await _context.Books
                .Where(b => b.IsBorrowed && b.BorrowedByUserId == userId)
                .Include(b => b.Author) // ✅ Dodano kako bi prikazali podatke o autoru
                .ToListAsync();

            return View(books);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(SavedBooks));
        }

        public async Task<IActionResult> DueSoon()
        {
            var today = DateTime.Now;
            var dueSoonBooks = await _context.Books.Where(b => b.IsBorrowed && b.ReturnDate.HasValue && b.ReturnDate.Value <= today.AddDays(3)).ToListAsync();
            return View(dueSoonBooks);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var dueDate = book.ReturnDate ?? DateTime.Now.AddDays(14);
            return View(book);
        }
        [Authorize]
        public async Task<IActionResult> MyLibrary()
        {
            var userId = _userManager.GetUserId(User); // ✅ Dohvaćanje ID-a trenutno prijavljenog korisnika

            var borrowedBooks = await _context.Books
                .Include(b => b.Author) // Uključuje podatke o autoru
                .Where(b => b.IsBorrowed && b.BorrowedByUserId == userId)
                .ToListAsync();

            return View(borrowedBooks);
        }


    }
}