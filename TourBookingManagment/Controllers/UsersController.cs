using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == userDto.Username);

            if (existingUser != null)
            {
                existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

                var existingDetails = await _context.UserDetails
                    .FirstOrDefaultAsync(ud => ud.UserId == existingUser.UserId);

                if (existingDetails != null)
                {
                    existingDetails.FirstName = userDto.FirstName;
                    existingDetails.LastName = userDto.LastName;
                    existingDetails.Email = userDto.Email;
                    existingDetails.DateOfBirth = userDto.DateOfBirth;
                    existingDetails.Gender = userDto.Gender;
                    existingDetails.Nationality = userDto.Nationality;
                    existingDetails.Phone = userDto.Phone;
                    existingDetails.Street = userDto.Street;
                    existingDetails.City = userDto.City;
                    existingDetails.State = userDto.State;
                    existingDetails.ZipCode = userDto.ZipCode;
                    existingDetails.Country = userDto.Country;
                }
                else
                {
                    _context.UserDetails.Add(new UserDetails
                    {
                        UserId = existingUser.UserId,
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
                    });
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "User details updated successfully!" });
            }

            var newUser = new User
            {
                Username = userDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            _context.UserDetails.Add(new UserDetails
            {
                UserId = newUser.UserId,
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
            });

            await _context.SaveChangesAsync();
            return Ok(new { message = "User created successfully!" });
        }
    }
}