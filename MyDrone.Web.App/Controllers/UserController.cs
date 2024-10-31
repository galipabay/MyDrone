﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyDrone.Kernel.Models;
using MyDrone.Kernel.Services;
using MyDrone.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

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

        #region Login

        // GET: User/Login
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.HideLayoutSections = true;
            return View();
        }

        // POST: User/Login
        [HttpPost]
        public async Task<IActionResult> Login(string Identifier, string Password)
        {
            if (string.IsNullOrWhiteSpace(Identifier) || string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı ve şifre boş olamaz.");
                return View();
            }

            var user = await _context.User.FirstOrDefaultAsync(u =>
                (u.MailAddress == Identifier || u.TelNo == Identifier) && u.Password == Password);

            if (user != null)
            {
                // Başarılı giriş için yönlendirme
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
                return View();
            }
        }

        #endregion

        #region Register

        // GET: UserDto/Register
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.HideLayoutSections = true;
            return View();
        }

        // POST: UserDto/Register
        [HttpPost]
        public async Task<IActionResult> Register(UserDto userDto, string confirmPassword)
        {

            // Eğer telefon numarası veya e-posta adresi boşsa hata döndür
            if (string.IsNullOrWhiteSpace(userDto.TelNo) || string.IsNullOrWhiteSpace(userDto.MailAddress))
            {
                ModelState.AddModelError(string.Empty, "Telefon numarası ve e-posta adresi gereklidir.");
                return View(userDto);
            }

            // Veritabanında aynı e-posta veya telefon numarası ile kayıtlı bir kullanıcı var mı kontrol et
            var existingUser = await _context.User
                .FirstOrDefaultAsync(u => u.MailAddress == userDto.MailAddress || u.TelNo == userDto.TelNo);

            if (existingUser != null)
            {
                // Hata mesajı döndür ve aynı e-posta ya da telefon numarası olan kullanıcıya izin verme
                if (existingUser.MailAddress == userDto.MailAddress)
                {
                    ModelState.AddModelError("MailAddress", "Bu e-posta adresiyle kayıtlı bir kullanıcı zaten var.");
                }
                if (existingUser.TelNo == userDto.TelNo)
                {
                    ModelState.AddModelError("TelNo", "Bu telefon numarasıyla kayıtlı bir kullanıcı zaten var.");
                }
                return View(userDto);
            }

            if (ModelState.IsValid)
            {
                if (userDto.Password != confirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Şifreler eşleşmiyor.");
                    return View(userDto);
                }

                // Kullanıcıyı veritabanına ekleme
                var newUser = _mapper.Map<User>(userDto); // UserDto'dan User'a haritalama
                newUser.CreatedDate = DateTime.Now;
                _context.User.Add(newUser);
                await _context.SaveChangesAsync();

                // Başarılı kayıt sonrasında giriş ekranına yönlendirme yap
                return RedirectToAction("Login");
            }
            // Model hatalıysa, aynı sayfayı yeniden göster
            return View(userDto);
        }

        #endregion

        /// <summary>
        /// Profil fotografini dbden ceken metot
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserProfileImage(int userId)
        {
            // Kullanıcıyı ID ile veritabanında bul
            var user = _context.User.FirstOrDefault(u => u.Id == userId);

            if (user != null && user.Image != null)
            {
                // Eğer kullanıcıda bir profil fotoğrafı varsa onu döndür
                return File(user.Image, "image/jpeg"); // "image/jpeg" dosya tipi olabilir, duruma göre değişebilir
            }
            else
            {
                // Varsayılan profil fotoğrafı döndür
                var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "default-profile.png");
                var defaultImage = System.IO.File.ReadAllBytes(defaultImagePath);
                return File(defaultImage, "image/png");
            }
        }

        // GET: UserDtoController
        public ActionResult Index()
        {
            return View();
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
