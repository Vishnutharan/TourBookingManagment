namespace TourBookingManagment.Model
{
    public class NotificationRequest
    {
        public int BookingId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}