using System.ComponentModel.DataAnnotations;

namespace TourBookingManagment.Model
{
    public class Coupons
    {
        [Key]
        public int Id { get; set; } // Primary Key

        [Required, MaxLength(50)]
        public string Code { get; set; }

        [Required, Range(0, 100)]
        public decimal DiscountPercentage { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MinimumAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MaximumDiscount { get; set; }

        [Range(0, int.MaxValue)]
        public int UsageLimit { get; set; }

        public int UsageCount { get; set; }

        public bool IsActive { get; set; }
    }
}
