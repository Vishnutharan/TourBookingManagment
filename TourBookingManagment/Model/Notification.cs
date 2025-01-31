namespace TourBookingManagment.Model
{
    public class Notification
    {
        public int Id { get; set; }
        public int BookingId { get; set; } 
        public string Message { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}