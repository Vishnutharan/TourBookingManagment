using Microsoft.Extensions.Caching.Memory;
using TourBookingManagment.Services;
using TourBookingManagment.Model;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;


namespace TourBookingManagment.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(IMemoryCache cache, IHttpClientFactory httpClientFactory, ILogger<CurrencyService> logger)
        {
            _cache = cache;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<CurrencyConversionModel> GetCachedExchangeRate(string fromCurrency, string toCurrency)
        {
            string cacheKey = $"ExchangeRate_{fromCurrency}_{toCurrency}";
            if (!_cache.TryGetValue(cacheKey, out CurrencyConversionModel rate))
            {
                rate = await FetchExchangeRate(fromCurrency, toCurrency);
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));
                _cache.Set(cacheKey, rate, cacheOptions);
            }
            return rate;
        }

        public async Task CacheExchangeRate(string fromCurrency, string toCurrency, decimal rate)
        {
            string cacheKey = $"ExchangeRate_{fromCurrency}_{toCurrency}";
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));
            _cache.Set(cacheKey, new CurrencyConversionModel { Rate = rate }, cacheOptions);
        }

        public async Task<bool> ValidateCurrency(string currency)
        {
            var validCurrencies = new HashSet<string> { "usd", "eur", "gbp" }; // Add more as needed
            return validCurrencies.Contains(currency.ToLower());
        }

        private async Task<CurrencyConversionModel> FetchExchangeRate(string fromCurrency, string toCurrency)
        {
            // Implement exchange rate API call here
            throw new NotImplementedException();
        }
    }
}