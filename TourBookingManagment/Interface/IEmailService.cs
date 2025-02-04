namespace TourBookingManagment.Interface
{
    public interface IEmailService
    {
        Task SendBookingConfirmationAsync(int bookingId);
    }
}
