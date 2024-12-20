using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TourBookingManagment.Model
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(100)]
        public string StripePaymentIntentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(4)]
        public string? LastFourDigits { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? ErrorMessage { get; set; }

        public string? Currency { get; set; } = "usd";

        [StringLength(100)]
        public string? CustomerEmail { get; set; }
    }
}
