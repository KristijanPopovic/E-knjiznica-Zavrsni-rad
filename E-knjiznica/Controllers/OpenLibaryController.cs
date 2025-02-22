﻿using E_knjiznica.Data;
using E_knjiznica.Models;
using E_knjiznica.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;

namespace E_knjiznica.Controllers
{
    public class OpenLibraryController : Controller
    {
        private readonly OpenLibraryService _openLibraryService;
        private readonly LibraryDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<IdentityUser> _userManager; // ✅ Dodano

        public OpenLibraryController(OpenLibraryService openLibraryService, LibraryDbContext context, IHttpClientFactory httpClientFactory, UserManager<IdentityUser> userManager)
        {
            _openLibraryService = openLibraryService;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager; // ✅ Inicijalizacija UserManager-a

        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            var books = await _openLibraryService.SearchBooksAsync(query);
            return View("SearchResults", books); // ✅ Pravilno prosljeđivanje liste knjiga
        }


        [HttpPost]
        public async Task<IActionResult> SaveBook(string title, string author, string publishedYear, string coverUrl)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author))
            {
                return Content("❌ Naslov i autor su obavezni.");
            }

            var existingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Name == author);
            if (existingAuthor == null)
            {
                existingAuthor = new Author { Name = author };
                _context.Authors.Add(existingAuthor);
                await _context.SaveChangesAsync();
            }

            var userId = _userManager.GetUserId(User); // ✅ Trenutni korisnik

            if (!_context.Books.Any(b => b.Title == title && b.BorrowedByUserId == userId))
            {
                var book = new Book
                {
                    Title = title,
                    Author = existingAuthor,
                    PublishedYear = publishedYear ?? "Nepoznato",
                    CoverUrl = coverUrl ?? "/images/default_cover.jpg",
                    Genre = "Nepoznat žanr",
                    IsBorrowed = true,                 // ✅ Oznaka da je knjiga posuđena
                    BorrowedByUserId = userId,         // ✅ Dodavanje ID-a korisnika
                    BorrowedDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddDays(30)
                };

                _context.Books.Add(book);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("SavedBooks", "Books"); // ✅ Prikaz posuđenih knjiga
        }

        public async Task<IActionResult> ImportBooks()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://openlibrary.org/subjects/popular.json?limit=30");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(json);

                foreach (var bookData in data.works)
                {
                    string title = bookData.title;
                    string authorName = bookData.authors?[0]?.name ?? "Nepoznati autor";
                    string publishedYear = bookData.first_publish_year?.ToString() ?? "Nepoznato";
                    string coverUrl = bookData.cover_id != null
                        ? $"https://covers.openlibrary.org/b/id/{bookData.cover_id}-L.jpg"
                        : "default_cover.jpg";

                    var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == authorName)
                                 ?? new Author { Name = authorName };

                    if (!_context.Books.Any(b => b.Title == title && b.Author.Name == authorName))
                    {
                        var newBook = new Book
                        {
                            Title = title,
                            Author = author,
                            PublishedYear = publishedYear,
                            CoverUrl = coverUrl,
                            Genre = "Nepoznato",
                            IsBorrowed = false,
                            BorrowedDate = DateTime.Now,
                            ReturnDate = DateTime.Now.AddDays(30)
                        };

                        _context.Books.Add(newBook);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "✅ Uspješno je uvezeno 30 knjiga!";
            }
            else
            {
                TempData["ErrorMessage"] = "❌ Došlo je do pogreške prilikom uvoza knjiga.";
            }

            return RedirectToAction("Index", "Books");
        }

    }
}
