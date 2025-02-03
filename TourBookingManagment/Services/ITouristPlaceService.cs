using TourBookingManagment.Model;

namespace TourBookingManagment.Services
{
    public interface ITouristPlaceService
    {
        Task<IEnumerable<TouristPlace>> GetAllPlacesAsync();
        Task<IEnumerable<TouristPlace>> GetPlacesByCountryAsync(string countryId);
        Task<TouristPlace> GetPlaceByIdAsync(string id);
        Task<TouristPlace> CreatePlaceAsync(TouristPlace place);
        Task<TouristPlace> UpdatePlaceAsync(string id, TouristPlace place);
        Task DeletePlaceAsync(string id);
    }
}