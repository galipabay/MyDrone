using Microsoft.EntityFrameworkCore;
using MyDrone.Kernel.Models;
using MyDrone.Kernel.Services;
using MyDrone.Types;

namespace MyDrone.Business.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateNotificationAsync(int receiverId, string message,
                                               int? senderId = null,
                                               string? relatedUrl = null,
                                               NotificationType type = NotificationType.General)
        {
            var notification = new Notification
            {
                ReceiverUserId = receiverId,
                SenderUserId = senderId,
                Message = message,
                RelatedUrl = relatedUrl,
                Type = type
            };

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(int userId, int count = 5)
        {
            return await _context.Notifications
                .Where(n => n.ReceiverUserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.ReceiverUserId == userId && !n.IsRead);
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        // Implemente edilmemiş methodların doldurulması:
        public async Task CreateAsync(string message, NotificationType type)
        {
            // Örnek: Varsayılan bir receiverId ile bildirim oluştur
            // (Gerçek projede receiverId parametre olarak alınmalı!)
            var defaultReceiverId = 1; // Örnek ID, projenize göre değiştirin
            await CreateNotificationAsync(defaultReceiverId, message, null, null, type);
        }

        public async Task<dynamic> GetUnreadCountAsync()
        {
            // Örnek: Tüm kullanıcıların okunmamış bildirim sayısını döndür
            // Veya başka bir mantık uygula
            var unreadCounts = await _context.Notifications
                .Where(n => !n.IsRead)
                .GroupBy(n => n.ReceiverUserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    UnreadCount = g.Count()
                })
                .ToListAsync();

            return unreadCounts;
        }
        public async Task<IEnumerable<Notification>> GetRecentNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.ReceiverUserId == userId)
                .OrderByDescending(n => n.CreatedDate)
                .Take(5) // en son 5 bildirim mesela
                .ToListAsync();
        }
    }
}