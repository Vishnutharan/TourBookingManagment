using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TourBookingManagment.Database;
using TourBookingManagment.Model;
using System.Text.Json; // Required for JSON serialization

namespace TourBookingManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristPlacesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TouristPlacesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TouristPlace>>> GetTouristPlaces()
        {
            return await _context.TouristPlaces.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TouristPlace>> GetTouristPlace(int id)
        {
            var touristPlace = await _context.TouristPlaces.FindAsync(id);
            if (touristPlace == null) return NotFound();
            return touristPlace;
        }

        [HttpPost]
        public async Task<ActionResult<TouristPlace>> PostTouristPlace(TouristPlace touristPlace)
        {
            // Check if the CountryId exists in the Countries table
            var country = await _context.Countries.FindAsync(touristPlace.CountryId);
            if (country == null)
            {
                return BadRequest($"Country with ID {touristPlace.CountryId} does not exist.");
            }

            _context.TouristPlaces.Add(touristPlace);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTouristPlace), new { id = touristPlace.PlaceId }, touristPlace);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutTouristPlace(int id, TouristPlace touristPlace)
        {
            if (id != touristPlace.PlaceId) return BadRequest();
            _context.Entry(touristPlace).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTouristPlace(int id)
        {
            var touristPlace = await _context.TouristPlaces.FindAsync(id);
            if (touristPlace == null) return NotFound();
            _context.TouristPlaces.Remove(touristPlace);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var filePath = Path.Combine("wwwroot", "uploads", file.FileName);

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the file path or URL as response
            return Ok($"uploads/{file.FileName}");
        }
    }
}