using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourBookingManagment.Database;
using TourBookingManagment.Model;

namespace TourBookingManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("package/{tourPackageId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews(int tourPackageId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.TourPackageId == tourPackageId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(reviews);
        }

        [HttpPost("submit")]
        public async Task<ActionResult<Review>> SubmitReview([FromBody] Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            review.CreatedAt = DateTime.UtcNow;
            review.Date = DateTime.UtcNow;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetReviews),
                new { tourPackageId = review.TourPackageId },
                review
            );
        }
    }
}