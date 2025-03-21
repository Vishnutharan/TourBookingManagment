using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourBookingManagment.Database;
using TourBookingManagment.Model;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;

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

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] Review review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            var existingReview = await _context.Reviews.FindAsync(id);
            if (existingReview == null)
            {
                return NotFound();
            }

            existingReview.TourPackageId = review.TourPackageId;
            existingReview.CustomerName = review.CustomerName;
            existingReview.Rating = review.Rating;
            existingReview.ReviewText = review.ReviewText;
            existingReview.UserImage = review.UserImage;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                var reviews = await _context.Reviews
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Reviews");

                // Add headers
                worksheet.Cell(1, 1).Value = "Review ID";
                worksheet.Cell(1, 2).Value = "Tour Package ID";
                worksheet.Cell(1, 3).Value = "Customer Name";
                worksheet.Cell(1, 4).Value = "Rating";
                worksheet.Cell(1, 5).Value = "Review Text";
                //worksheet.Cell(1, 6).Value = "Created At";
                //worksheet.Cell(1, 7).Value = "User Image";
                //worksheet.Cell(1, 8).Value = "Date";

                // Add data
                for (int i = 0; i < reviews.Count; i++)
                {
                    var review = reviews[i];
                    int row = i + 2;

                    worksheet.Cell(row, 1).Value = review.Id;
                    worksheet.Cell(row, 2).Value = review.TourPackageId;
                    worksheet.Cell(row, 3).Value = review.CustomerName;
                    worksheet.Cell(row, 4).Value = review.Rating;
                    worksheet.Cell(row, 5).Value = review.ReviewText;
                    //worksheet.Cell(row, 6).Value = review.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
                    //worksheet.Cell(row, 7).Value = review.UserImage ?? "";
                    //worksheet.Cell(row, 8).Value = review.Date.ToString("yyyy-MM-dd HH:mm:ss");
                }

                // Format columns
                worksheet.Column(4).Style.NumberFormat.NumberFormatId = 1;
                worksheet.Column(6).Style.NumberFormat.Format = "yyyy-mm-dd hh:mm:ss";
                worksheet.Column(8).Style.NumberFormat.Format = "yyyy-mm-dd hh:mm:ss";

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(
                    stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Reviews.xlsx"
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error exporting to Excel", detail = ex.Message });
            }
        }

        [HttpPost("import-excel")]
        public async Task<IActionResult> ImportFromExcel(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded");

                if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                    return BadRequest("Invalid file format. Please upload an Excel file (.xlsx)");

                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1); // Skip header row

                var validationErrors = new List<string>();
                var updatedReviews = new List<Review>();

                foreach (var row in rows)
                {
                    try
                    {
                        var reviewId = row.Cell(1).GetValue<int>();
                        var review = await _context.Reviews.FindAsync(reviewId);

                        if (review == null)
                        {
                            validationErrors.Add($"Review ID {reviewId} not found");
                            continue;
                        }

                        var validationResult = ValidateReviewData(row);
                        if (!validationResult.IsValid)
                        {
                            validationErrors.Add($"Review ID {reviewId}: {validationResult.Error}");
                            continue;
                        }

                        UpdateReviewFromExcelRow(review, row);
                        updatedReviews.Add(review);
                    }
                    catch (Exception ex)
                    {
                        validationErrors.Add($"Error processing row {row.RowNumber()}: {ex.Message}");
                    }
                }

                if (updatedReviews.Any())
                {
                    await _context.SaveChangesAsync();
                }

                return Ok(new
                {
                    message = $"Successfully updated {updatedReviews.Count} reviews",
                    errors = validationErrors
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error importing from Excel", detail = ex.Message });
            }
        }

        // Private helper methods
        private static (bool IsValid, string Error) ValidateReviewData(IXLRow row)
        {
            try
            {
                // Validate Tour Package ID
                if (row.Cell(2).GetValue<int>() <= 0)
                    return (false, "Tour Package ID must be greater than 0");

                // Validate Customer Name
                var customerName = row.Cell(3).GetString();
                if (string.IsNullOrEmpty(customerName) || customerName.Length > 100)
                    return (false, "Customer Name is required and must not exceed 100 characters");

                // Validate Rating
                var rating = row.Cell(4).GetValue<int>();
                if (rating < 1 || rating > 5)
                    return (false, "Rating must be between 1 and 5");

                // Validate Review Text
                var reviewText = row.Cell(5).GetString();
                if (string.IsNullOrEmpty(reviewText) || reviewText.Length > 1000)
                    return (false, "Review Text is required and must not exceed 1000 characters");

                // Validate User Image (optional)
                var userImage = row.Cell(7).GetString();
                if (!string.IsNullOrEmpty(userImage) && userImage.Length > 500)
                    return (false, "User Image URL must not exceed 500 characters");

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Validation error: {ex.Message}");
            }
        }

        private static void UpdateReviewFromExcelRow(Review review, IXLRow row)
        {
            review.TourPackageId = row.Cell(2).GetValue<int>();
            review.CustomerName = row.Cell(3).GetString();
            review.Rating = row.Cell(4).GetValue<int>();
            review.ReviewText = row.Cell(5).GetString();
            review.UserImage = row.Cell(7).GetString();
            // Don't update CreatedAt and Date from Excel
        }
    }
}