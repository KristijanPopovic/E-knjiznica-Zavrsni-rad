using Microsoft.AspNetCore.Mvc;
using E_knjiznica.Data;
using E_knjiznica.Models;
using System.Threading.Tasks;

namespace E_knjiznica.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly LibraryDbContext _context;

        public ReviewsController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(int bookId, string userName, int rating)
        {
            if (rating < 1 || rating > 10)
            {
                return BadRequest("Ocjena mora biti između 1 i 10.");
            }

            var review = new Review
            {
                BookId = bookId,
                UserName = userName,
                Rating = rating
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Books", new { id = bookId });
        }
    }
}
