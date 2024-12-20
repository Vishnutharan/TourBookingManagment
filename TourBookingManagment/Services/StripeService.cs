using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using TourBookingManagment.Database;
using TourBookingManagment.DTOs;
using TourBookingManagment.Model;

namespace TourBookingManagment.Services
{
    public class StripeService : IStripeService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly ILogger<StripeService> _logger;

        public StripeService(
            IConfiguration configuration,
            AppDbContext context,
            ILogger<StripeService> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
            Stripe.StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<StripeCheckoutResponse> CreateCheckoutSessionAsync(PaymentRequestDto request)
        {
            try
            {
                var options = new Stripe.PaymentIntentCreateOptions
                {
                    Amount = Convert.ToInt64(request.Amount * 100),
                    Currency = request.Currency,
                    PaymentMethodTypes = new List<string> { "card" },
                    ReceiptEmail = request.CustomerEmail,
                    Metadata = new Dictionary<string, string>
                    {
                        { "CustomerEmail", request.CustomerEmail ?? "" }
                    }
                };

                var service = new Stripe.PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);

                var transaction = new TourBookingManagment.Model.Transaction
                {
                    Amount = request.Amount,
                    StripePaymentIntentId = paymentIntent.Id,
                    Status = paymentIntent.Status,
                    Currency = request.Currency,
                    CustomerEmail = request.CustomerEmail
                };

                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                return new StripeCheckoutResponse
                {
                    SessionId = paymentIntent.Id,
                    ClientSecret = paymentIntent.ClientSecret
                };
            }
            catch (Stripe.StripeException ex)
            {
                _logger.LogError(ex, "Stripe error occurred while creating checkout session");
                throw;
            }
        }

        public async Task<Stripe.PaymentIntent> ConfirmPaymentAsync(string paymentIntentId)
        {
            var service = new Stripe.PaymentIntentService();
            var paymentIntent = await service.GetAsync(paymentIntentId);

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.StripePaymentIntentId == paymentIntentId);

            if (transaction != null)
            {
                transaction.Status = paymentIntent.Status;
                transaction.LastFourDigits = paymentIntent.PaymentMethod?.Card?.Last4;
                await _context.SaveChangesAsync();
            }

            return paymentIntent;
        }

        public async Task<Stripe.PaymentIntent> CancelPaymentAsync(string paymentIntentId)
        {
            var service = new Stripe.PaymentIntentService();
            return await service.CancelAsync(paymentIntentId);
        }

        public async Task<TourBookingManagment.Model.Transaction> GetTransactionAsync(string paymentIntentId)
        {
            return await _context.Transactions
                .FirstOrDefaultAsync(t => t.StripePaymentIntentId == paymentIntentId);
        }
    }
}