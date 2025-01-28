// TourBookingManagment/DTOs/UserDto.cs
namespace TourBookingManagment.DTOs
{
    public class UserDto // For authentication requests
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}