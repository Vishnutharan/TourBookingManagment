using TourBookingManagment.Model;

namespace TourBookingManagment.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
