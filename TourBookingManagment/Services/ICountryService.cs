using TourBookingManagment.Model;

namespace TourBookingManagment.Services
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<Country> GetCountryByIdAsync(string id);
        Task<Country> CreateCountryAsync(Country country);
        Task<Country> UpdateCountryAsync(string id, Country country);
        Task DeleteCountryAsync(string id);
    }
}