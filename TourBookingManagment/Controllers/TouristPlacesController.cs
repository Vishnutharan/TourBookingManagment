//using Microsoft.AspNetCore.Mvc;
//using TourBookingManagment.Model;
//using TourBookingManagment.Services;

//namespace TourBookingManagment.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class TouristPlacesController : ControllerBase
//    {
//        private readonly ITouristPlaceService _touristPlaceService;
//        private readonly ILogger<TouristPlacesController> _logger;

//        public TouristPlacesController(ITouristPlaceService touristPlaceService, ILogger<TouristPlacesController> logger)
//        {
//            _touristPlaceService = touristPlaceService;
//            _logger = logger;
//        }

//        // GET: api/TouristPlaces
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<TouristPlace>>> GetTouristPlaces()
//        {
//            try
//            {
//                var places = await _touristPlaceService.GetAllPlacesAsync();
//                return Ok(places);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting all tourist places");
//                return StatusCode(500, "Internal server error while retrieving tourist places");
//            }
//        }

//        // GET: api/TouristPlaces/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<TouristPlace>> GetTouristPlace(string id)
//        {
//            try
//            {
//                var place = await _touristPlaceService.GetPlaceByIdAsync(id);

//                if (place == null)
//                {
//                    return NotFound($"Tourist place with ID {id} not found");
//                }

//                return Ok(place);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting tourist place with id {Id}", id);
//                return StatusCode(500, "Internal server error while retrieving the tourist place");
//            }
//        }

//        // GET: api/TouristPlaces/country/5
//        [HttpGet("country/{countryId}")]
//        public async Task<ActionResult<IEnumerable<TouristPlace>>> GetPlacesByCountry(string countryId)
//        {
//            try
//            {
//                var places = await _touristPlaceService.GetPlacesByCountryAsync(countryId);
//                return Ok(places);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting tourist places for country {CountryId}", countryId);
//                return StatusCode(500, "Internal server error while retrieving tourist places");
//            }
//        }

//        // POST: api/TouristPlaces
//        [HttpPost]
//        public async Task<ActionResult<TouristPlace>> CreateTouristPlace(TouristPlace touristPlace)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var createdPlace = await _touristPlaceService.CreatePlaceAsync(touristPlace);
//                return CreatedAtAction(nameof(GetTouristPlace), new { id = createdPlace.PlaceId }, createdPlace);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error creating tourist place");
//                return StatusCode(500, "Internal server error while creating the tourist place");
//            }
//        }

//        // PUT: api/TouristPlaces/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateTouristPlace(string id, TouristPlace touristPlace)
//        {
//            try
//            {

//                if (placeId.ToString() != touristPlace.PlaceId.ToString())
//                {
//                    return BadRequest("ID mismatch");
//                }


//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var updatedPlace = await _touristPlaceService.UpdatePlaceAsync(id, touristPlace);

//                if (updatedPlace == null)
//                {
//                    return NotFound($"Tourist place with ID {id} not found");
//                }

//                return Ok(updatedPlace);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating tourist place with id {Id}", id);
//                return StatusCode(500, "Internal server error while updating the tourist place");
//            }
//        }


//        // DELETE: api/TouristPlaces/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteTouristPlace(string id)
//        {
//            try
//            {
//                await _touristPlaceService.DeletePlaceAsync(id);
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error deleting tourist place with id {Id}", id);
//                return StatusCode(500, "Internal server error while deleting the tourist place");
//            }
//        }
//    }
//}