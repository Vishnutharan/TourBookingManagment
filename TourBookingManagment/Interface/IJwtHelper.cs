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
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ??
                        user.FindFirstValue(JwtRegisteredClaimNames.Sub);

            return userId;
        }
    }
}