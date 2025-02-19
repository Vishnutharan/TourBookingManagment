using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourBookingManagment.Database;
using TourBookingManagment.Model;

namespace TourBookingManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CountriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(string id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.CountryId == id);
            if (country == null) return NotFound();
            return country;
        }

        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(Country country)
        {
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCountry), new { id = country.CountryId }, country);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(string id, Country country)
        {
            if (id != country.CountryId) return BadRequest();
            _context.Entry(country).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(string id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null) return NotFound();
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}