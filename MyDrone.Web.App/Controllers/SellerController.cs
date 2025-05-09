﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyDrone.Kernel.Models;
using MyDrone.Kernel.Services;
using MyDrone.Types;
using MyDrone.Web.App.Models;
using System.Security.Claims;

namespace MyDrone.Web.App.Controllers
{
    public class SellerController : Controller
    {
        private readonly IService<Device> _sellerDeviceService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IDeviceService _deviceService;

        public SellerController(IService<Device> sellerDeviceService, IMapper mapper, AppDbContext context, IEmailService emailService, IDeviceService deviceService)
        {
            _sellerDeviceService = sellerDeviceService;
            _mapper = mapper;
            _context = context;
            _emailService = emailService;
            _deviceService = deviceService;

        }

        #region Devices
        /// <summary>
        /// Kullanıcının cihazlarını listeler
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Devices()
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)); // ASP.NET Core'da kullanıcı ID'si almak için

            var user = _context.Users.Find(userId); // Kullanıcı bilgilerini çekiyoruz

            if (user == null || !user.IsSeller) // Eğer kullanıcı bulunamazsa veya seller değilse
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            var devices = await _context.Devices.Where(d => d.UserId == userId).ToListAsync();
            return View(devices);
        }

        #endregion

        #region DeviceDetail
        /// <summary>
        /// Cihaz detay sayfası
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DeviceDetail(int id)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Kullanıcı ID'sini alıyoruz
            var device = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id); // Cihazı veritabanından alıyoruz


            if (device == null)
            {
                return NotFound();
            }

            // Favori kontrolü için asenkron işlem
            var isFavorited = await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.DeviceId == id);

            bool isSeller = device.UserId == userId; // Cihazın sahibi mi?  
            var deviceDetailViewModel = new DeviceDetailViewModel
            {
                Device = device,
                IsFavorited = isFavorited,
                IsSeller = isSeller
            };
            return View(deviceDetailViewModel);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int deviceId)
        {
            var device = await _context.Devices.FindAsync(deviceId);

            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _context.Users.FindAsync(userId);

            if (device == null || user == null)
            {
                return Json(new { success = false, message = "Cihaz veya kullanıcı bulunamadı." });
            }

            // Kullanıcı seller mı? Eğer seller ise, kendi cihazını favoriye ekleyemez
            var sellerUserId = device.UserId;  // Cihazın sahibi

            if (userId == sellerUserId)
            {
                return Json(new { success = false, message = "Kendi ürününüzü favoriye ekleyemezsiniz." });
            }

            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.DeviceId == deviceId);

            if (favorite != null)
            {
                // Eğer zaten favoriyse, favoriden çıkar
                _context.Favorites.Remove(favorite);
            }
            else
            {
                // Eğer favoriye eklenmemişse, favoriye ekle
                _context.Favorites.Add(new Favorite
                {
                    UserId = userId,
                    DeviceId = deviceId
                });
            }
            await _context.SaveChangesAsync();

            // Yeni favori durumu kontrol et
            var isFavorited = await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.DeviceId == deviceId);

            return RedirectToAction("DeviceDetail", new { id = deviceId });

        }
        #endregion

        #region DeviceAdd
        /// <summary>
        /// Cihaz ekleme sayfası
        /// </summary>
        /// <param name="selectedBrand"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DeviceAdd(string? selectedBrand) // Boş gelebilir
        {
            // Tüm markaları al
            var brands = _context.DeviceAttributes
                .Where(d => d.AttributeType == "Marka")
                .Select(d => d.AttributeValue)
                .Distinct()
                .ToList();

            ViewBag.Brands = new SelectList(brands);

            // Eğer marka seçilmemişse boş model listesi gönder
            ViewBag.Models = new SelectList(new List<string>());

            if (!string.IsNullOrEmpty(selectedBrand))
            {
                var chainNo = _context.DeviceAttributes
                    .Where(m => m.AttributeType == "Marka" && m.AttributeValue == selectedBrand)
                    .Select(m => m.DeviceChainNo)
                    .FirstOrDefault();

                if (chainNo > 0) // Eğer böyle bir marka varsa
                {
                    var models = _context.DeviceAttributes
                        .Where(d => d.AttributeType == "Model" && d.DeviceChainNo == chainNo)
                        .Select(d => d.AttributeValue)
                        .Distinct()
                        .ToList();

                    ViewBag.Models = new SelectList(models);
                }

                ViewData["SelectedBrand"] = selectedBrand;

            }

            return View();
        }
        /// <summary>
        /// Cihaz ekleme işlemi
        /// </summary>
        /// <param name="device"></param>
        /// <param name="imageFile"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeviceAdd(Device device, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                // **Seçilen markaya göre modelleri getir**
                List<string> models = new List<string>();
                if (!string.IsNullOrEmpty(device.Brand))
                {
                    models = _context.DeviceAttributes
                        .Where(a => a.AttributeType == "Model" &&
                                    _context.DeviceAttributes.Any(b => b.AttributeType == "Marka" &&
                                                                        b.AttributeValue == device.Brand &&
                                                                        b.DeviceChainNo == a.DeviceChainNo))
                        .Select(a => a.AttributeValue)
                        .Distinct()
                        .ToList();
                }
                ViewBag.Models = new SelectList(models);

                // Giriş yapan kullanıcının ID'sini al
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdString, out int userId))
                {
                    return BadRequest(); // Kullanıcı ID hatası
                }

                // Fotoğraf verisini al ve byte dizisine dönüştür
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(memoryStream);
                        device.Image = memoryStream.ToArray(); // Fotoğrafı byte dizisine ata
                    }
                }

                device.CreatedDate = DateTime.Now;
                device.UpdatedDate = DateTime.Now;
                device.UserId = userId; // Kullanıcıyı ata

                // Cihaz numarasını otomatik al
                device.DeviceNo = await _deviceService.GenerateNextDeviceNumberAsync();

                // Veritabanına ekle
                _context.Devices.Add(device);
                await _context.SaveChangesAsync();

                return RedirectToAction("Devices"); // Liste sayfasına yönlendir
            }
            return View(device); // Model hatalıysa sayfayı tekrar göster
        }

        #endregion

        #region EditDevice
        /// <summary>
        /// Ürün düzenleme sayfası
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditDevice(int deviceNo)
        {
            var device = _context.Devices.FirstOrDefault(d => d.DeviceNo == deviceNo);
            if (device == null)
            {
                return NotFound();
            }
            return View("DeviceEdit", device);
        }

        /// <summary>
        /// Ürün düzenleme işlemi
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDevice(Device device)
        {
            if (ModelState.IsValid)
            {
                _context.Devices.Update(device);
                await _context.SaveChangesAsync();
                return RedirectToAction("Devices");
            }
            return View(device);
        }

        #endregion

        #region DeleteDevice

        /// <summary>
        /// Ürün silme işlemi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteDeviceConfirmed(int id, string deleteReason)
        {
            var device = await _context.Devices.FindAsync(id); // Ürünü getir
            if (device == null) return NotFound();

            device.IsDeleted = true;
            device.DeletedDate = DateTime.Now;
            device.DeletedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            device.DeleteReason = deleteReason;

            _context.SaveChanges();
            TempData["DeleteMessage"] = "Ürün başarıyla silindi.";
            return RedirectToAction("Devices"); // Liste sayfasına yönlendir
        }

        #endregion
    }
}