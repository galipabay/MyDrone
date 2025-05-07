// ViewComponents/NotificationsViewComponent.cs
using Microsoft.AspNetCore.Mvc;
using MyDrone.Kernel.Models;
using MyDrone.Kernel.Services;
using System.Security.Claims;

public class NotificationsViewComponent : ViewComponent
{
    private readonly INotificationService _notificationService;

    public NotificationsViewComponent(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (userId != null)
        {
            var notifications = await _notificationService.GetRecentNotificationsAsync(Convert.ToInt32(userId));
            return View(notifications);
        }
        return View(new List<Notification>());
    }
}