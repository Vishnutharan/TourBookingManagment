using Microsoft.Extensions.Caching.Memory;
using TourBookingManagment.Services;
using TourBookingManagment.Model;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using TourBookingManagment.Interface;


namespace TourBookingManagment.Services
{
    public class CurrencyService : ICurrencyService
    {
        public Task<CurrencyConversionModel> GetCachedExchangeRate(string fromCurrency, string toCurrency)
        {
            // Implementation logic for retrieving cached exchange rate
            throw new NotImplementedException();
        }

        public Task CacheExchangeRate(string fromCurrency, string toCurrency, decimal rate)
        {
            // Implementation logic for caching exchange rate
            throw new NotImplementedException();
        }

        private readonly HashSet<string> _validCurrencies = new HashSet<string>
         {
        "USD", "EUR", "GBP", "CAD", "AUD" // Add more as needed
        };

        public async Task<bool> ValidateCurrency(string currency)
        {
            return _validCurrencies.Contains(currency.ToUpper());
        }
    }
}