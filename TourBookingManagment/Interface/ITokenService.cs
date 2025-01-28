using TourBookingManagment.Model;

namespace TourBookingManagment.Interface
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
