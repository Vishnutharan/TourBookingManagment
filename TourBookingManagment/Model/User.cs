using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourBookingManagment.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }

        // Navigation property
        public virtual UserDetails? UserDetails { get; set; }
    }
}