namespace TourBookingManagment.Interface
{
    public interface IEmailService
    {
        Task SendBookingNotificationAsync(string toEmail, string subject, string body);
    }
}
