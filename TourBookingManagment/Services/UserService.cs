using TourBookingManagment.Database;
using TourBookingManagment.Model;

namespace TourBookingManagment.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUser(User user)
        {
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                user.PasswordHash = null; // Set null for empty or missing PasswordHash
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
