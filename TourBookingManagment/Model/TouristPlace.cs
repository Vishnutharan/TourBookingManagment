using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TourBookingManagment.Model
{
    public class TouristPlace
    {
        [Key]
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }

        // Foreign key to Country
        [Required]
        [ForeignKey("Country")]
        public string CountryId { get; set; }
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

        // TouristGuideDetails
        public string GuideName { get; set; }
        public string Experience { get; set; }
        public IEnumerable<string> LanguagesSpoken { get; set; }
        public string ContactNumber { get; set; }
        public IEnumerable<string> Specialization { get; set; }
    }
}
