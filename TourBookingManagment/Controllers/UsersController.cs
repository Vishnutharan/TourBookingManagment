using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TourBookingManagment.Database;
using TourBookingManagment.DTOs;
using TourBookingManagment.Model;

namespace TourBookingManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserCreateDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check for both JWT standard claim and custom claim
            var userIdClaim = User.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier ||
                c.Type == "userId" ||
                c.Type == "sub");

            // If no user ID claim is found in the token, create a new user
            if (userIdClaim == null)
            {
                var newUser = new User
                {
                    Username = userDto.Username,
                    UserDetails = new UserDetails
                    {
                        FirstName = userDto.FirstName,
                        LastName = userDto.LastName,
                        Email = userDto.Email,
                        DateOfBirth = userDto.DateOfBirth,
                        Gender = userDto.Gender,
                        Nationality = userDto.Nationality,
                        Phone = userDto.Phone,
                        Street = userDto.Street,
                        City = userDto.City,
                        State = userDto.State,
                        ZipCode = userDto.ZipCode,
                        Country = userDto.Country,
                        CreatedAt = DateTime.Now
                    }
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return Ok(new { message = "User created successfully!", userId = newUser.UserId });
            }

            // If user ID claim exists, update existing user
            int userId;
            if (!int.TryParse(userIdClaim.Value, out userId))
                return BadRequest(new { message = "Invalid user ID format" });

            var existingUser = await _context.Users
                .Include(u => u.UserDetails)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (existingUser == null)
                return NotFound(new { message = "User not found" });

            // Update user details
            if (existingUser.UserDetails == null)
            {
                existingUser.UserDetails = new UserDetails
                {
                    UserId = userId,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    DateOfBirth = userDto.DateOfBirth,
                    Gender = userDto.Gender,
                    Nationality = userDto.Nationality,
                    Phone = userDto.Phone,
                    Street = userDto.Street,
                    City = userDto.City,
                    State = userDto.State,
                    ZipCode = userDto.ZipCode,
                    Country = userDto.Country,
                    CreatedAt = DateTime.Now
                };
                _context.UserDetails.Add(existingUser.UserDetails);
            }
            else
            {
                existingUser.UserDetails.FirstName = userDto.FirstName;
                existingUser.UserDetails.LastName = userDto.LastName;
                existingUser.UserDetails.Email = userDto.Email;
                existingUser.UserDetails.DateOfBirth = userDto.DateOfBirth;
                existingUser.UserDetails.Gender = userDto.Gender;
                existingUser.UserDetails.Nationality = userDto.Nationality;
                existingUser.UserDetails.Phone = userDto.Phone;
                existingUser.UserDetails.Street = userDto.Street;
                existingUser.UserDetails.City = userDto.City;
                existingUser.UserDetails.State = userDto.State;
                existingUser.UserDetails.ZipCode = userDto.ZipCode;
                existingUser.UserDetails.Country = userDto.Country;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "User details updated successfully!" });
        }
        // Add these methods to your existing UsersController class

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCreateDto>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.UserDetails)
                .Select(u => new UserCreateDto
                {
                    Username = u.Username ?? string.Empty,
                    FirstName = u.UserDetails != null ? u.UserDetails.FirstName : string.Empty,
                    LastName = u.UserDetails != null ? u.UserDetails.LastName : string.Empty,
                    Email = u.UserDetails != null ? u.UserDetails.Email ?? string.Empty : string.Empty,
                    DateOfBirth = u.UserDetails != null ? u.UserDetails.DateOfBirth : DateTime.MinValue,
                    Gender = u.UserDetails != null ? u.UserDetails.Gender : string.Empty,
                    Nationality = u.UserDetails != null ? u.UserDetails.Nationality : string.Empty,
                    Phone = u.UserDetails != null ? u.UserDetails.Phone : string.Empty,
                    Street = u.UserDetails != null ? u.UserDetails.Street : string.Empty,
                    City = u.UserDetails != null ? u.UserDetails.City : string.Empty,
                    State = u.UserDetails != null ? u.UserDetails.State : string.Empty,
                    ZipCode = u.UserDetails != null ? u.UserDetails.ZipCode : string.Empty,
                    Country = u.UserDetails != null ? u.UserDetails.Country : string.Empty
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserCreateDto>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserDetails)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound(new { message = "User not found!" });
            }

            var userDto = new UserCreateDto
            {
                Username = user.Username ?? string.Empty,
                FirstName = user.UserDetails?.FirstName ?? string.Empty,
                LastName = user.UserDetails?.LastName ?? string.Empty,
                Email = user.UserDetails?.Email ?? string.Empty,
                DateOfBirth = user.UserDetails?.DateOfBirth ?? DateTime.MinValue,
                Gender = user.UserDetails?.Gender ?? string.Empty,
                Nationality = user.UserDetails?.Nationality ?? string.Empty,
                Phone = user.UserDetails?.Phone ?? string.Empty,
                Street = user.UserDetails?.Street ?? string.Empty,
                City = user.UserDetails?.City ?? string.Empty,
                State = user.UserDetails?.State ?? string.Empty,
                ZipCode = user.UserDetails?.ZipCode ?? string.Empty,
                Country = user.UserDetails?.Country ?? string.Empty
            };

            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserCreateDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users
                .Include(u => u.UserDetails)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound(new { message = "User not found!" });
            }

            // Update user password if provided
            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }

            // Update username if provided and different
            if (!string.IsNullOrEmpty(userDto.Username) && user.Username != userDto.Username)
            {
                // Check if new username is already taken
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == userDto.Username && u.UserId != id);

                if (existingUser != null)
                {
                    return BadRequest(new { message = "Username is already taken!" });
                }

                user.Username = userDto.Username;
            }

            if (user.UserDetails == null)
            {
                user.UserDetails = new UserDetails
                {
                    UserId = user.UserId,
                    CreatedAt = DateTime.Now
                };
                _context.UserDetails.Add(user.UserDetails);
            }

            // Update user details
            user.UserDetails.FirstName = userDto.FirstName;
            user.UserDetails.LastName = userDto.LastName;
            user.UserDetails.Email = userDto.Email;
            user.UserDetails.DateOfBirth = userDto.DateOfBirth;
            user.UserDetails.Gender = userDto.Gender;
            user.UserDetails.Nationality = userDto.Nationality;
            user.UserDetails.Phone = userDto.Phone;
            user.UserDetails.Street = userDto.Street;
            user.UserDetails.City = userDto.City;
            user.UserDetails.State = userDto.State;
            user.UserDetails.ZipCode = userDto.ZipCode;
            user.UserDetails.Country = userDto.Country;

            await _context.SaveChangesAsync();
            return Ok(new { message = "User updated successfully!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserDetails)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound(new { message = "User not found!" });
            }

            if (user.UserDetails != null)
            {
                _context.UserDetails.Remove(user.UserDetails);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully!" });
        }
    }
}