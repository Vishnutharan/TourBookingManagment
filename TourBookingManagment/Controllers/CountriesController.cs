//using Microsoft.AspNetCore.Mvc;
//using TourBookingManagment.Model;
//using TourBookingManagment.Services;

//namespace TourBookingManagment.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class CountriesController : ControllerBase
//    {
//        private readonly ICountryService _countryService;
//        private readonly ILogger<CountriesController> _logger;

//        public CountriesController(ICountryService countryService, ILogger<CountriesController> logger)
//        {
//            _countryService = countryService;
//            _logger = logger;
//        }

//        // GET: api/Countries
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
//        {
//            try
//            {
//                var countries = await _countryService.GetAllCountriesAsync();
//                return Ok(countries);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting all countries");
//                return StatusCode(500, "Internal server error while retrieving countries");
//            }
//        }

//        // GET: api/Countries/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Country>> GetCountry(string id)
//        {
//            try
//            {
//                var country = await _countryService.GetCountryByIdAsync(id);

//                if (country == null)
//                {
//                    return NotFound($"Country with ID {id} not found");
//                }

//                return Ok(country);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting country with id {Id}", id);
//                return StatusCode(500, "Internal server error while retrieving the country");
//            }
//        }

//        // POST: api/Countries
//        [HttpPost]
//        public async Task<ActionResult<Country>> CreateCountry(Country country)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var createdCountry = await _countryService.CreateCountryAsync(country);
//                return CreatedAtAction(nameof(GetCountry), new { id = createdCountry.Id }, createdCountry);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error creating country");
//                return StatusCode(500, "Internal server error while creating the country");
//            }
//        }

//        // PUT: api/Countries/5

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateCountry(string id, Country country)
//        {
//            try
//            {
//                if (!Guid.TryParse(id, out Guid countryId))
//                {
//                    return BadRequest("Invalid ID format");
//                }

//                if (countryId != country.Id)
//                {
//                    return BadRequest("ID mismatch");
//                }

//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var updatedCountry = await _countryService.UpdateCountryAsync(id, country);

//                if (updatedCountry == null)
//                {
//                    return NotFound($"Country with ID {id} not found");
//                }

//                return Ok(updatedCountry);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating country with id {Id}", id);
//                return StatusCode(500, "Internal server error while updating the country");
//            }
//        }


//        // DELETE: api/Countries/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteCountry(string id)
//        {
//            try
//            {
//                await _countryService.DeleteCountryAsync(id);
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error deleting country with id {Id}", id);
//                return StatusCode(500, "Internal server error while deleting the country");
//            }
//        }
//    }
//}