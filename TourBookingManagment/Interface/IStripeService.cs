using TourBookingManagment.DTOs;
using TourBookingManagment.Model;

namespace TourBookingManagment.Interface
{
    public interface IStripeService
    {
        Task<StripeCheckoutResponse> CreateCheckoutSessionAsync(PaymentRequestDto request);
        Task<Stripe.PaymentIntent> ConfirmPaymentAsync(string sessionId);
        Task<Stripe.PaymentIntent> CancelPaymentAsync(string sessionId);
        Task<TourBookingManagment.Model.Transaction> GetTransactionAsync(string paymentIntentId);
    }
}
