//using Microsoft.EntityFrameworkCore;
//using TourBookingManagment.Database;
//using TourBookingManagment.Model;

//namespace TourBookingManagment.Services
//{
//    public class CountryService : ICountryService
//    {
//        private readonly AppDbContext _context;

//        public CountryService(AppDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
//        {
//            return await _context.Countries.ToListAsync();
//        }

//        public async Task<Country> GetCountryByIdAsync(string id)
//        {
//            return await _context.Countries.FindAsync(id);
//        }

//        public async Task<Country> CreateCountryAsync(Country country)
//        {
//            country.CountryId = Guid.NewGuid().ToString();
//            _context.Countries.Add(country);
//            await _context.SaveChangesAsync();
//            return country;
//        }

//        public async Task<Country> UpdateCountryAsync(string id, Country country)
//        {
//            var existingCountry = await _context.Countries.FindAsync(id);
//            if (existingCountry == null) return null;

//            existingCountry.Name = country.Name;
//            existingCountry.Description = country.Description;
//            existingCountry.ImageUrl = country.ImageUrl;
//            existingCountry.FeaturedScore = country.FeaturedScore;

//            await _context.SaveChangesAsync();
//            return existingCountry;
//        }

//        public async Task DeleteCountryAsync(string id)
//        {
//            var country = await _context.Countries.FindAsync(id);
//            if (country != null)
//            {
//                _context.Countries.Remove(country);
//                await _context.SaveChangesAsync();
//            }
//        }
//    }
//}