using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDrone.Kernel.Models;
using MyDrone.Kernel.Services;
using MyDrone.Kernel.UnitOfWork;
using MyDrone.Types;
using MyDrone.Web.App.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace MyDrone.Web.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService<Device> _deviceService;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IService<Device> deviceService, AppDbContext context)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _deviceService = deviceService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var devices = await _context.Device.ToListAsync();

            // Kullanıcı giriş yaptıysa favori kontrolü yap, giriş yapmadıysa boş liste ata
            List<int> favoriteDeviceIds = new List<int>();
            List<int> recentlyViewedDeviceIds = new List<int>();

            if (User.Identity.IsAuthenticated)
            {
                var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

                // Favori ilanları çek
                favoriteDeviceIds = await _context.Favorite
                    .Where(f => f.UserId == userId)
                    .Select(f => f.DeviceId)
                    .ToListAsync();

                // Son görüntülenen ilanları çek (en fazla 5 adet)
                recentlyViewedDeviceIds = await _context.RecentlyViewed
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.ViewedAt)
                    .Take(5)
                    .Select(r => r.DeviceId)
                    .ToListAsync();
            }

            // ViewModel oluştur
            var viewModel = devices.Select(device => new DeviceDetailViewModel
            {
                Device = device,
                IsFavorited = favoriteDeviceIds.Contains(device.Id),
                IsRecentlyViewed = recentlyViewedDeviceIds.Contains(device.Id) // Güncelleme burada
            }).ToList();

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
