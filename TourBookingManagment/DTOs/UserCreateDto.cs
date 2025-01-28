// TourBookingManagment/DTOs/UserCreateDto.cs
namespace TourBookingManagment.DTOs
{
    public class UserCreateDto
    {
        public int UserId { get; set; }  // Remove UserId
        // Add username and password
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Existing properties
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}