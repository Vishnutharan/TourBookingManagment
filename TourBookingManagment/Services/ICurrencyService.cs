using System.Threading.Tasks;
using TourBookingManagment.Model;

namespace TourBookingManagment.Services
{
    public interface ICurrencyService
    {
        Task<CurrencyConversionModel> GetCachedExchangeRate(string fromCurrency, string toCurrency);
        Task CacheExchangeRate(string fromCurrency, string toCurrency, decimal rate);
        Task<bool> ValidateCurrency(string currency);
    }
}
