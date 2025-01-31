using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TourBookingManagment.Interface;

namespace TourBookingManagment.Hub

{
    namespace TourBookingManagment.Hub
    {
        public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
        {
            private readonly INotificationService _notificationService;

            public NotificationHub(INotificationService notificationService)
            {
                _notificationService = notificationService;
            }
            public async Task SendNotification(string message)
            {
                await Clients.All.SendAsync("ReceiveNotification", message);
            }

            public async Task SendBookingNotification(int bookingId, string message)
            {
                await Clients.All.SendAsync("ReceiveNotification", bookingId, message);
                await _notificationService.SaveNotification(bookingId, message);
            }

            public async Task JoinGroup(string groupName)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }

            public async Task SendToGroup(string groupName, string message)
            {
                await Clients.Group(groupName).SendAsync("ReceiveGroupNotification", message);
            }
        }
    }
}
