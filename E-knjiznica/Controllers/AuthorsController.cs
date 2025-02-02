using Microsoft.AspNetCore.Mvc;
using E_knjiznica.Data;
using E_knjiznica.Models;
using E_knjiznica.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_knjiznica.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly OpenLibraryAuthorService _authorService;

        public AuthorsController(LibraryDbContext context, OpenLibraryAuthorService authorService)
        {
            _context = context;
            _authorService = authorService;
        }

        // ✅ Prikaz svih autora
        public async Task<IActionResult> Index()
        {
            return View(await _context.Authors.Include(a => a.Books).ToListAsync());
        }

        // ✅ Prikaz forme za dodavanje novog autora
        public IActionResult Create()
        {
            return View();
        }

        // ✅ Dodavanje novog autora
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            if (ModelState.IsValid)
            {
                // ✅ Ako je OpenLibraryId prazan, postavi zadanu vrijednost
                author.OpenLibraryId ??= "N/A";
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // ✅ Prikaz detalja o autoru (povezano s Open Library)
        public async Task<IActionResult> Details(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
                return NotFound();

            // ✅ Dohvaćanje biografije s Open Library
            var biography = await _authorService.GetAuthorBiographyAsync(author.OpenLibraryId);

            ViewBag.Biography = biography;

            return View(author);
        }


        // ✅ Brisanje autora
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
