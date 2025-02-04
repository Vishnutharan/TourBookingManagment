using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using TourBookingManagment.Interface;
using TourBookingManagment.Model;
using TourBookingManagment.Database;

namespace TourBookingManagment.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly AppDbContext _context;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger, AppDbContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
        }

        public async Task SendBookingConfirmationAsync(int bookingId)
        {
            try
            {
                var booking = await _context.BookingDetails
                    .FirstOrDefaultAsync(b => b.Id == bookingId);

                if (booking == null)
                {
                    _logger.LogError($"Booking not found with ID: {bookingId}");
                    return;
                }

                var smtpSettings = _configuration.GetSection("SmtpSettings");
                using var client = new SmtpClient(smtpSettings["Server"], int.Parse(smtpSettings["Port"]))
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
                    EnableSsl = true
                };

                string messageBody = $@"
Dear {booking.Name},

Thank you for your booking! Below are your booking details:

Booking Information:
-------------------
Booking ID: {booking.Id}
Booking Date: {booking.BookingDate:dd/MM/yyyy}
Travel Date: {booking.DateOfTravel:dd/MM/yyyy}
Number of People: {booking.NumberOfPeople}
Destinations: {booking.Placess}

Payment Details:
--------------
Total Amount: ${booking.TotalAmount:N2}
Tax: ${booking.Tax:N2}
Final Amount: ${booking.FinalAmount:N2}

Contact Information:
------------------
Name: {booking.Name}
Email: {booking.Email}
Phone: {booking.Phone}

Status: {booking.Status}

Thank you for choosing our service. If you have any questions, please don't hesitate to contact us.

Best regards,
Tour Booking Team";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings["SenderEmail"], smtpSettings["SenderName"]),
                    Subject = $"Booking Confirmation - ID: {booking.Id}",
                    Body = messageBody,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(booking.Email);
                await client.SendMailAsync(mailMessage);

                _logger.LogInformation($"Booking confirmation email sent successfully to {booking.Email} for booking ID: {bookingId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send booking confirmation email for booking ID: {bookingId}");
                throw;
            }
        }
    }
}