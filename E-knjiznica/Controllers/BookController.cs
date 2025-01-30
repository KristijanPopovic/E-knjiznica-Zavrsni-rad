﻿using Microsoft.AspNetCore.Mvc;
using E_knjiznica.Data;
using E_knjiznica.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_knjiznica.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
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
                await _context.SaveChangesAsync(); // This line saves the book to the database
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
    }
}
