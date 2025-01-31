using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourBookingManagment.Database;
using TourBookingManagment.Model;

[ApiController]
[Route("api/[controller]")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BookingController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<BookingController> _logger;

    public BookingController(AppDbContext context, ILogger<BookingController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateBooking([FromBody] BookingDetails booking)
    {
        try
        {
            var user = await _context.Users.FindAsync(booking.UserId);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {booking.UserId} not found");
                return NotFound(new { message = "User not found" });
            }

            var newBooking = new BookingDetails
            {
                UserId = booking.UserId,
                Name = booking.Name,
                Email = booking.Email,
                Phone = booking.Phone,
                DateOfTravel = booking.DateOfTravel,
                NumberOfPeople = booking.NumberOfPeople,
                TotalAmount = booking.TotalAmount,
                Tax = booking.Tax,
                FinalAmount = booking.FinalAmount,
                BookingDate = DateTime.UtcNow,
                Status = "Pending",
                Placess = booking.Placess
            };

            await _context.BookingDetails.AddAsync(newBooking);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking created successfully", bookingId = newBooking.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating booking");
            return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
        }
    }
}