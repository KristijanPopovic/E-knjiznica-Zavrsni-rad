using Microsoft.AspNetCore.Mvc;
using E_knjiznica.Data;
using E_knjiznica.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace E_knjiznica.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchTitle, string searchAuthor, string searchGenre, int? searchYear)
        {
            try
            {
                var books = _context.Books.AsQueryable();

                if (!string.IsNullOrEmpty(searchTitle))
                    books = books.Where(b => b.Title.Contains(searchTitle));

                if (!string.IsNullOrEmpty(searchAuthor))
                    books = books.Where(b => b.Author != null && b.Author.Name.Contains(searchAuthor));


                if (!string.IsNullOrEmpty(searchGenre))
                    books = books.Where(b => b.Genre.Contains(searchGenre));

                if (searchYear.HasValue)
                    books = books.Where(b => b.PublishedYear == searchYear.Value.ToString());

                foreach (var book in await books.ToListAsync())
                {
                    if (book.BorrowedDate == null)
                    {
                        Console.WriteLine($"Knjiga s ID: {book.Id} ima NULL za BorrowedDate.");
                    }
                    if (book.ReturnDate == null)
                    {
                        Console.WriteLine($"Knjiga s ID: {book.Id} ima NULL za ReturnDate.");
                    }
                }
                try
                {
                    return View(await books.ToListAsync());
                }
                catch (Exception ex)
                {
                    return Content($"Greška prilikom učitavanja podataka: {ex.Message}\n\n📋 Detalji: {ex.StackTrace}");
                }


                return View(await books.ToListAsync());
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

        public async Task<IActionResult> Borrow(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null || book.IsBorrowed)
            {
                return NotFound();
            }

            book.IsBorrowed = true;
            book.BorrowedDate = DateTime.Now;

            book.ReturnDate = DateTime.Now.AddDays(30);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
            book.BorrowedByUserId = null;
            book.ReturnDate = null;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SavedBooks()
        {
            var books = await _context.Books.ToListAsync();
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
            var dueSoonBooks = await _context.Books
                .Where(b => b.IsBorrowed && b.ReturnDate.HasValue && b.ReturnDate.Value <= today.AddDays(3))
                .ToListAsync();

            return View(dueSoonBooks);
        }
    }
}
