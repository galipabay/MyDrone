using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MyDrone.Kernel.Services;
using MyDrone.Kernel.UnitOfWork;
using MyDrone.Types;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MyDrone.Web.App.Controllers
{
    public class BaseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        protected readonly AppDbContext _context;

        public BaseController(IUnitOfWork unitOfWork, INotificationService notificationService, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _context = context;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var user = await _context.Users
    .FirstOrDefaultAsync(u => u.Id == Convert.ToInt32(userId));

                if (user != null)
                {
                    ViewBag.UserName = user.Name;
                    ViewBag.UserSurname = user.Surname;
                    ViewBag.UserId = user.Id;
                    ViewBag.UserEmail = user.MailAddress;

                    var notifications = await _notificationService.GetRecentNotificationsAsync(Convert.ToInt32(userId));
                    TempData["RecentNotifications"] = JsonConvert.SerializeObject(notifications);
                    ViewBag.UnreadCount = await _notificationService.GetUnreadCountAsync(user.Id);
                }
            }
            await next();
        }
    }
}
