using E_knjiznica.Data;
using E_knjiznica.Models;
using E_knjiznica.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_knjiznica.Controllers
{
    public class OpenLibraryController : Controller
    {
        private readonly OpenLibraryService _openLibraryService;
        private readonly LibraryDbContext _context; // ✅ Pristup bazi podataka

        // ✅ Ispravljena inicijalizacija
        public OpenLibraryController(OpenLibraryService openLibraryService, LibraryDbContext context)
        {
            _openLibraryService = openLibraryService;
            _context = context;
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                ViewBag.ErrorMessage = "Molimo unesite pojam za pretragu.";
                return View();
            }

            var books = await _openLibraryService.SearchBooksAsync(query);
            return View(books);
        }

        [HttpPost]
        [HttpPost]
        [HttpPost]
        [HttpPost]
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> SaveBook(string title, string author, string publishedYear, string coverUrl, string genre)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author))
            {
                return Content("❌ Greška: Naslov i autor su obavezni.");
            }
            var existingAuthor = _context.Authors.FirstOrDefault(a => a.Name == author);
            if (existingAuthor == null)
            {
                existingAuthor = new Author { Name = author };
                _context.Authors.Add(existingAuthor);
                await _context.SaveChangesAsync();
            }

            var book = new Book
            {
                Title = title ?? "Nepoznato",
                Author = existingAuthor, // ✅ Ispravno
                PublishedYear = !string.IsNullOrEmpty(publishedYear) ? publishedYear : "Nepoznato",
                CoverUrl = !string.IsNullOrEmpty(coverUrl) ? coverUrl : "default_cover.jpg",
                Genre = !string.IsNullOrEmpty(genre) ? genre : "Nepoznat žanr",
                IsBorrowed = false,

                // ✅ Zamjena DateTime.MinValue s podržanim datumom
                BorrowedDate = new DateTime(1753, 1, 1),
                ReturnDate = new DateTime(1753, 1, 1)
            };

            try
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Books");
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message ?? "Nema dodatnih detalja.";
                return Content($"❌ Greška prilikom spremanja knjige: {dbEx.Message}\n\n📋 Unutarnja greška: {innerException}");
            }
            catch (Exception ex)
            {
                return Content($"❌ Neočekivana greška: {ex.Message}\n\n📋 Detalji: {ex.StackTrace}");
            }
        }



    }



}


