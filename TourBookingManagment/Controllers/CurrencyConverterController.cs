using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TourBookingManagment.Model;
using TourBookingManagment.Services;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TourBookingManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyConverterController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CurrencyConverterController> _logger;

        public CurrencyConverterController(IHttpClientFactory httpClientFactory, ILogger<CurrencyConverterController> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        [HttpGet("convert")]
        public async Task<IActionResult> ConvertCurrency(
            [FromQuery] string fromCurrency,
            [FromQuery] string toCurrency,
            [FromQuery] decimal amount)
        {
            if (string.IsNullOrWhiteSpace(fromCurrency) || string.IsNullOrWhiteSpace(toCurrency) || amount <= 0)
            {
                return BadRequest("Invalid input parameters.");
            }

            try
            {
                var apiUrl = $"https://api.exchangerate-api.com/v4/latest/{fromCurrency}";
                _logger.LogInformation("Calling external API with URL: {apiUrl}", apiUrl);

                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseBody);
                var rates = jsonResponse["rates"] as JObject;

                if (rates == null || !rates.ContainsKey(toCurrency))
                {
                    return BadRequest($"Unable to fetch exchange rate for {fromCurrency} to {toCurrency}.");
                }

                var exchangeRate = rates[toCurrency].Value<decimal>();
                var convertedAmount = amount * exchangeRate;

                return Ok(new
                {
                    FromCurrency = fromCurrency,
                    ToCurrency = toCurrency,
                    Amount = amount,
                    ConvertedAmount = convertedAmount,
                    ExchangeRate = exchangeRate
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error while calling external API.");
                return StatusCode(500, $"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
