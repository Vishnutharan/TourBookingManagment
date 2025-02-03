using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourBookingManagment.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string? Username { get; set; }

        public string PasswordHash { get; set; } = string.Empty;

        // Navigation property
        public virtual UserDetails UserDetails { get; set; }
    }
}