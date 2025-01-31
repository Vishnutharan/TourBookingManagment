using TourBookingManagment.Model;

namespace TourBookingManagment.Interface
{
    public interface INotificationService
    {
        Task SaveNotification(int bookingId, string message);
        Task<List<Notification>> GetNotificationsByBookingId(int bookingId);
    }
}