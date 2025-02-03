using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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

    [HttpGet("get")]
    public async Task<IActionResult> GetUserBookings()
    {
        try
        {
            // Get all bookings (no user authentication check)
            var bookings = await _context.BookingDetails.ToListAsync();

            if (bookings == null || bookings.Count == 0)
            {
                return NotFound(new { message = "No bookings found" });
            }

            return Ok(bookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving bookings");
            return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
        }
    }


    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetBooking(int id)
    {
        try
        {
            var booking = await _context.BookingDetails.FindAsync(id);
            if (booking == null)
            {
                _logger.LogWarning($"Booking with ID {id} not found");
                return NotFound(new { message = "Booking not found" });
            }

            return Ok(booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving booking");
            return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
        }
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateBooking(int id, [FromBody] BookingDetails booking)
    {
        try
        {
            var existingBooking = await _context.BookingDetails.FindAsync(id);
            if (existingBooking == null)
            {
                _logger.LogWarning($"Booking with ID {id} not found");
                return NotFound(new { message = "Booking not found" });
            }

            // Update the properties as needed
            existingBooking.Name = booking.Name ?? existingBooking.Name;
            existingBooking.Email = booking.Email ?? existingBooking.Email;
            existingBooking.Phone = booking.Phone ?? existingBooking.Phone;
            existingBooking.DateOfTravel = booking.DateOfTravel ?? existingBooking.DateOfTravel;
            existingBooking.NumberOfPeople = booking.NumberOfPeople;
            existingBooking.TotalAmount = booking.TotalAmount ?? existingBooking.TotalAmount;
            existingBooking.Tax = booking.Tax ?? existingBooking.Tax;
            existingBooking.FinalAmount = booking.FinalAmount ?? existingBooking.FinalAmount;
            existingBooking.Placess = booking.Placess ?? existingBooking.Placess;
            existingBooking.Status = booking.Status ?? existingBooking.Status;

            // Save the updated booking
            _context.BookingDetails.Update(existingBooking);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking updated successfully", bookingId = existingBooking.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating booking");
            return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        try
        {
            var booking = await _context.BookingDetails.FindAsync(id);
            if (booking == null)
            {
                _logger.LogWarning($"Booking with ID {id} not found");
                return NotFound(new { message = "Booking not found" });
            }

            _context.BookingDetails.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting booking");
            return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
        }
    }
}