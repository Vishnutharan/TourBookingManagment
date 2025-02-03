//using Microsoft.EntityFrameworkCore;
//using TourBookingManagment.Database;
//using TourBookingManagment.Model;

//namespace TourBookingManagment.Services
//{
//    public class TouristPlaceService : ITouristPlaceService
//    {
//        private readonly AppDbContext _context;

//        public TouristPlaceService(AppDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<TouristPlace>> GetAllPlacesAsync()
//        {
//            return await _context.TouristPlaces.ToListAsync();
//        }

//        public async Task<IEnumerable<TouristPlace>> GetPlacesByCountryAsync(string countryId)
//        {
//            return await _context.TouristPlaces
//                .Where(p => p.CountryId == countryId)
//                .ToListAsync();
//        }

//        public async Task<TouristPlace> GetPlaceByIdAsync(string id)
//        {
//            return await _context.TouristPlaces.FindAsync(id);
//        }

//        public async Task<TouristPlace> CreatePlaceAsync(TouristPlace place)
//        {
//            place.Id = Guid.NewGuid().ToString();
//            place.PlaceId = Guid.NewGuid().ToString();
//            _context.TouristPlaces.Add(place);
//            await _context.SaveChangesAsync();
//            return place;
//        }

//        public async Task<TouristPlace> UpdatePlaceAsync(string id, TouristPlace place)
//        {
//            var existingPlace = await _context.TouristPlaces.FindAsync(id);
//            if (existingPlace == null) return null;

//            existingPlace.Name = place.Name;
//            existingPlace.Description = place.Description;
//            existingPlace.ImageUrl = place.ImageUrl;
//            existingPlace.Rating = place.Rating;
//            existingPlace.Cost = place.Cost;
//            existingPlace.Highlights = place.Highlights;
//            existingPlace.BestTimeToVisit = place.BestTimeToVisit;
//            existingPlace.Duration = place.Duration;
//            existingPlace.Accommodation = place.Accommodation;
//            existingPlace.TravelDetails = place.TravelDetails;

//            await _context.SaveChangesAsync();
//            return existingPlace;
//        }

//        public async Task DeletePlaceAsync(string id)
//        {
//            var place = await _context.TouristPlaces.FindAsync(id);
//            if (place != null)
//            {
//                _context.TouristPlaces.Remove(place);
//                await _context.SaveChangesAsync();
//            }
//        }
//    }
//}