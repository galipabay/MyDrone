using MyDrone.Kernel.Models;

namespace MyDrone.Kernel.Services
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(int receiverId, string message,
                               int? senderId = null,
                               string? relatedUrl = null,
                               NotificationType type = NotificationType.General);
        Task<List<Notification>> GetUserNotificationsAsync(int userId, int count = 5);
        Task<int> GetUnreadCountAsync(int userId);
        Task MarkAsReadAsync(int notificationId);
        Task CreateAsync(string message, NotificationType type);
        Task<IEnumerable<Notification>> GetRecentNotificationsAsync(int userId);

    }
}
