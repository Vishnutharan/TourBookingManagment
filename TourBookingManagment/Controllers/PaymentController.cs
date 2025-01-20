using Microsoft.AspNetCore.Mvc;
using TourBookingManagment.DTOs;
using TourBookingManagment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TourBookingManagment.Model;
using TourBookingManagment.DTOs;
using TourBookingManagment.Services;
using System.Threading.Tasks;


namespace TourBookingManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IStripeService _stripeService;
        private readonly ICurrencyService _currencyService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IStripeService stripeService,ICurrencyService currencyService,ILogger<PaymentController> logger)
        {
            _stripeService = stripeService;
            _currencyService = currencyService;
            _logger = logger;
        }

        [HttpPost("create-checkout-session")]
        public async Task<ActionResult<PaymentResponseDto>> CreateCheckoutSession([FromBody] PaymentRequestDto request)
        {
            try
            {
                if (!await _currencyService.ValidateCurrency(request.Currency))
                {
                    return BadRequest(new { message = "Invalid currency" });
                }

                var result = await _stripeService.CreateCheckoutSessionAsync(request);
                return Ok(new PaymentResponseDto
                {
                    SessionId = result.SessionId,
                    ClientSecret = result.ClientSecret,
                    Amount = request.Amount,
                    Currency = request.Currency
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating checkout session");
                return StatusCode(500, new { message = "An error occurred while processing your request" });
            }
        }

        [HttpPost("confirm-payment")]
        public async Task<ActionResult<PaymentResponseDto>> ConfirmPayment(
            [FromBody] PaymentConfirmationDto confirmation)
        {
            try
            {
                var paymentIntent = await _stripeService.ConfirmPaymentAsync(confirmation.SessionId);
                var transaction = await _stripeService.GetTransactionAsync(confirmation.SessionId);

                return Ok(new PaymentResponseDto
                {
                    SessionId = paymentIntent.Id,
                    Status = paymentIntent.Status,
                    Amount = transaction?.Amount ?? 0,
                    Currency = transaction?.Currency ?? "usd"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming payment");
                return StatusCode(500, new { message = "An error occurred while confirming the payment" });
            }
        }

        [HttpPost("cancel-payment")]
        public async Task<IActionResult> CancelPayment([FromBody] PaymentConfirmationDto confirmation)
        {
            try
            {
                await _stripeService.CancelPaymentAsync(confirmation.SessionId);
                return Ok(new { message = "Payment cancelled successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling payment");
                return StatusCode(500, new { message = "An error occurred while cancelling the payment" });
            }
        }
    }
}