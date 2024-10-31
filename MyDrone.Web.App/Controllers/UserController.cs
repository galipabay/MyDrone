using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyDrone.Kernel.Models;
using MyDrone.Kernel.Services;
using MyDrone.Types;

namespace MyDrone.Web.App.Controllers
{
    public class UserController : Controller
    {

        private readonly IService<UserDto> _service;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;


        /// <summary>
        /// Dependency Injection yapiyoruz.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="mapper"></param>
        /// <param name="context"></param>
        /// 
        public UserController(IService<UserDto> service, IMapper mapper, AppDbContext context)
        {
            _service = service;
            _mapper = mapper;
            _context = context;

        }

        // GET: UserDto/Register
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.HideLayoutSections = true;
            return View();
        }

        // POST: UserDto/Login
        [HttpGet]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                
            }
            // Model hatalıysa, aynı sayfayı yeniden göster
            return View(userDto);
        }

        // POST: UserDto/Register
        [HttpPost]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                // UserDto'dan User'a dönüştür
                var user = _mapper.Map<User>(userDto);

                // Kullanıcıyı veritabanına ekleme
                _context.User.Add(user);
                await _context.SaveChangesAsync();

                // Başarılı bir kayıt sonrası başka bir sayfaya yönlendirme yapabilirsiniz.
                return RedirectToAction("Index", "Home"); // Ana sayfaya yönlendirme örneği
            }
            // Model hatalıysa, aynı sayfayı yeniden göster
            return View(userDto);
        }

        // GET: UserDtoController
        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Save()
        {
            var users = await _service.GetAllAsync();
            var usersDto = _mapper.Map<List<UserDto>>(users.ToList());
            ViewBag.students = new SelectList(usersDto, "Id", "TcNo");
            return View(usersDto);
        }

        // GET: UserDtoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserDtoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserDtoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserDtoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserDtoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserDtoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserDtoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
