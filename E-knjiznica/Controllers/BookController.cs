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

        // ✅ Updated Index method with search functionality
        public async Task<IActionResult> Index(string searchTitle, string searchAuthor, string searchGenre, int? searchYear)
        {
            var books = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(searchTitle))
            {
                books = books.Where(b => b.Title.Contains(searchTitle));
            }

            if (!string.IsNullOrEmpty(searchAuthor))
            {
                books = books.Where(b => b.Author.Contains(searchAuthor));
            }

            if (!string.IsNullOrEmpty(searchGenre))
            {
                books = books.Where(b => b.Genre.Contains(searchGenre));
            }

            if (searchYear.HasValue)
            {
                books = books.Where(b => b.PublishedYear == searchYear.Value.ToString());
            }


            return View(await books.ToListAsync());
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
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SavedBooks()
        {
            var books = await _context.Books.ToListAsync();
            return View(books);
        }
        // ✅ Brisanje spremljene knjige
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
    }
}
