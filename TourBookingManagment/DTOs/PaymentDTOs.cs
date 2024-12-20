using System.ComponentModel.DataAnnotations;

namespace TourBookingManagment.DTOs
{
    public class PaymentRequestDto
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; } = "usd";

        [EmailAddress]
        public string? CustomerEmail { get; set; }
    }

    public class PaymentConfirmationDto
    {
        [Required]
        public string SessionId { get; set; }
    }

    public class PaymentResponseDto
    {
        public string SessionId { get; set; }
        public string ClientSecret { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}