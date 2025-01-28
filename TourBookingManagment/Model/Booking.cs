using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourBookingManagment.Model
{
    public class Country
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int FeaturedScore { get; set; }
    }
     
    public class Review
    {
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TourPackageId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? CustomerName { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [MaxLength(1000)]
        public string? ReviewText { get; set; } // Add nullability

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? UserImage { get; set; } // Optional property for user avatar

        [Required]
        public DateTime Date { get; set; }
    }

    public class TouristPlace
    {
        [ForeignKey("User")]
        public int UserId { get; set; }

        public string? Id { get; set; }
        public string? CountryId { get; set; }
        public string? PlaceId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Cost { get; set; }
        public int Rating { get; set; }
        public List<string?>? Highlights { get; set; }
        public string? BestTimeToVisit { get; set; }
        public string? Duration { get; set; }

        // Accommodation details
        public string? HotelName { get; set; }
        public string? RoomType { get; set; }
        public string? SpecialRequests { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int? NumberOfRooms { get; set; }
        public string? OccupancyDetails { get; set; }

        // Travel details
        public string? TransportationMode { get; set; }
        public string? TravelDuration { get; set; }
        public decimal? TravelCost { get; set; }
    }

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