using TourBookingManagment.Model;

namespace TourBookingManagment.Interface
{
    public interface ICurrencyService
    {
        Task<CurrencyConversionModel> GetCachedExchangeRate(string fromCurrency, string toCurrency);
        Task CacheExchangeRate(string fromCurrency, string toCurrency, decimal rate);
        Task<bool> ValidateCurrency(string currency);
    }
}
