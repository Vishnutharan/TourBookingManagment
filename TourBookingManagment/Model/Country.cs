using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourBookingManagment.Model
{
    public class Country
    {
        [Key]
        public string CountryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal FeaturedScore { get; set; }
    }
}
