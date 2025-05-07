using Microsoft.AspNetCore.Mvc;
using MyDrone.Kernel.Services;
using System.Security.Claims;

namespace MyDrone.Web.App.Controllers
{
    // TODO: Login olan kullanicilara hali hazirda bildirim varsa gitmeli 
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IActionResult> GetUserNotifications()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return PartialView("_NotificationDropdown", notifications);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            return Ok();
        }
    }
}
