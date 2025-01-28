using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TourBookingManagment.Interface
{
    public interface IJwtHelper
    {
        string? GetUserIdFromToken(ClaimsPrincipal user);

    }
    public class JwtHelper : IJwtHelper
    {
        public string? GetUserIdFromToken(ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);
        }
    }
}
