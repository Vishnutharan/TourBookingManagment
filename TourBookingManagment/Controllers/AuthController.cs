using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TourBookingManagment.Database;
using TourBookingManagment.DTOs;
using TourBookingManagment.Model;

namespace TourBookingManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(AppDbContext context, IConfiguration config) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly IConfiguration _config = config;

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto request)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = hashedPassword
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials.");
            }

            var claims = new[]
            {
              new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // Must match User.FindFirstValue()
              new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()), // Optional but consistent
              new Claim(JwtRegisteredClaimNames.Name, user.Username)
            };

            // Rest of token generation code remains same
            var keyText = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(keyText))
            {
                return StatusCode(500, "JWT key is not configured.");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyText));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            expires: DateTime.Now.AddHours(1),
            claims: claims,
            signingCredentials: creds
         );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

    }
}