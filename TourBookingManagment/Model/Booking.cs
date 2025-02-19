using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourBookingManagment.Model
{
    public class BookingDetails
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //[Required]
        //public string? UserId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Status { get; set; }

        public string? Placess { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? DateOfTravel { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? FinalAmount { get; set; }
        public decimal? Tax { get; set; }
    }

    public class BookedPlace
    {
        public int Id { get; set; }
        public int BookingDetailsId { get; set; }
        public string? PlaceId { get; set; }
        public string? CountryId { get; set; }
        public string? Name { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
    }
}