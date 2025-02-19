using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TourBookingManagment.Hub.TourBookingManagment.Hub;
using TourBookingManagment.Interface;
using TourBookingManagment.Model;

namespace TourBookingManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationsController(
            INotificationService notificationService,
            IHubContext<NotificationHub> hubContext)
        {
            _notificationService = notificationService;
            _hubContext = hubContext;
        }

        [HttpGet("{bookingId}")]
        public async Task<ActionResult<List<Notification>>> GetNotificationsByBookingId(int bookingId)
        {
            try
            {
                var notifications = await _notificationService.GetNotificationsByBookingId(bookingId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateNotification([FromBody] NotificationRequest request)
        {
            try
            {
                await _notificationService.SaveNotification(request.BookingId, request.Message);
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", request.BookingId, request.Message);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("group")]
        public async Task<ActionResult> SendGroupNotification([FromBody] GroupNotificationRequest request)
        {
            try
            {
                await _hubContext.Clients.Group(request.GroupName).SendAsync("ReceiveGroupNotification", request.Message);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}