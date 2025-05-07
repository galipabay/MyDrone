using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyDrone.Kernel.Models;
using MyDrone.Kernel.Services;
using MyDrone.Types;
using MyDrone.Web.App.Models;

namespace MyDrone.Web.App.Controllers
{
    public class SearchController : Controller
    {
        private readonly IService<User> _userService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IService<Device> _sellerDeviceService;
        private readonly IDeviceService _deviceService;

        public SearchController(
            IService<User> userService,
            IMapper mapper,
            AppDbContext context,
            IEmailService emailService,
            IService<Device> sellerDeviceService,
            IDeviceService deviceService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _sellerDeviceService = sellerDeviceService ?? throw new ArgumentNullException(nameof(sellerDeviceService));
            _deviceService = deviceService ?? throw new ArgumentNullException(nameof(deviceService));
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Results
        [HttpGet]
        public IActionResult Results(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return RedirectToAction("Index", "Home");

            // TODO: SearchDevices metodundan veri gelmedi kontrol edilmesi lazim
            var matchingDevices = _deviceService.SearchDevices(query);
            // TODO: SearchUsers metodundan veri gelmedi kontrol edilmesi lazim
            var matchingUsers = _userService.SearchUsers(query);

            var viewModel = new SearchResultsViewModel
            {
                Query = query,
                Devices = matchingDevices,
                Users = matchingUsers
            };

            return View("SearchResults", viewModel);
        }

        [HttpGet]
        public JsonResult SearchUser(string query)
        {
            var matchingUsers = _userService.SearchUsers(query);
            return Json(matchingUsers);
        }


        #endregion
    }
}
