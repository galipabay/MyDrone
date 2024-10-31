using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyDrone.Kernel.Models;
using MyDrone.Kernel.Services;
using MyDrone.Types;

namespace MyDrone.Web.App.Controllers
{
    public class UserDtoController : Controller
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
        public UserDtoController(IService<UserDto> service, IMapper mapper, AppDbContext context)
        {
            _service = service;
            _mapper = mapper;
            _context = context;

        }

        // GET: UserDto/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: UserDto/Register
        [HttpPost]
        public async Task<IActionResult> Register(UserDto user)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcıyı veritabanına ekleme
                _context.UserDtos.Add(user);
                await _context.SaveChangesAsync();
                // Başarılı bir kayıt sonrası başka bir sayfaya yönlendirme yapabilirsiniz.
                return RedirectToAction("Index", "Home"); // Ana sayfaya yönlendirme örneği
            }
            // Model hatalıysa, aynı sayfayı yeniden göster
            return View(user);
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
