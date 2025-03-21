﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TourBookingManagment.Database;
using TourBookingManagment.Interface;
using TourBookingManagment.Model;
using TourBookingManagment.Hub;
using TourBookingManagment.Hub.TourBookingManagment.Hub;

namespace TourBookingManagment.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(AppDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task SaveNotification(int bookingId, string message, string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("UserName cannot be null or empty", nameof(userName));
            }

            var notification = new Notification
            {
                BookingId = bookingId,
                Message = message,
                UserName = userName,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetNotificationsByBookingId(int bookingId)
        {
            return await _context.Notifications
                .Where(n => n.BookingId == bookingId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task SaveNotification(int bookingId, string message)
        {
            throw new NotImplementedException();
        }
    }
}