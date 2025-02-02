using E_knjiznica.Data;
using E_knjiznica.Models;
using E_knjiznica.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_knjiznica.Controllers
{
    public class OpenLibraryController : Controller
    {
        private readonly OpenLibraryService _openLibraryService;
        private readonly LibraryDbContext _context; // ✅ Dodano za pristup bazi podataka
        public OpenLibraryController(OpenLibraryService openLibraryService)
        {
            _openLibraryService = openLibraryService;
        }

        // Prikaz forme za pretragu
        public IActionResult Search()
        {
            return View();
        }

        // Obrada pretrage nakon što korisnik unese pojam
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
        public IActionResult SaveBook(string title, string author, string publishedYear, string coverUrl)
        {
            var newBook = new Book
            {
                Title = title,
                Author = author,
                PublishedYear = publishedYear,
                CoverUrl = coverUrl
            };

            _context.Books.Add(newBook);
            _context.SaveChanges();

            return RedirectToAction("Search");
        }
    }
}
