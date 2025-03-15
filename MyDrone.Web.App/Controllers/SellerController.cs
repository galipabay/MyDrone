using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyDrone.Business.Services;
using MyDrone.Kernel.Models;
using MyDrone.Kernel.Services;
using MyDrone.Types;
using System.Drawing;
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

        public SellerController(IService<Device> sellerDeviceService, IMapper mapper, AppDbContext context, IEmailService emailService,IDeviceService deviceService)
        {
            _sellerDeviceService = sellerDeviceService;
            _mapper = mapper;
            _context = context;
            _emailService = emailService;
            _deviceService = deviceService;

        }
        #region Devices
        // Tüm cihazları listeleme
        public async Task<IActionResult> Devices()
        {
            var devices = await _sellerDeviceService.GetAllAsync();
            return View(devices);
        }

        #endregion

        #region DeviceDetail
        public IActionResult DeviceDetail(int id)
        {
            var device = _context.Device.FirstOrDefault(x => x.Id == id);

            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }
        #endregion

        #region DeviceAdd
        public IActionResult DeviceAdd()
        {
            // Markaları ve modelleri çekiyoruz
            var brands = _context.DeviceAttributes
                .Where(a => a.AttributeType == "Marka")
                .Select(a => a.AttributeValue)
                .Distinct()
                .ToList();

            var models = _context.DeviceAttributes
            .Where(a => a.AttributeType == "Model")
            .Select(a => a.AttributeValue)
            .Distinct()
            .ToList();

            // Verileri viewbag ile gonderiyoruz
            ViewBag.Brands = new SelectList(brands);
            ViewBag.Models = new SelectList(models);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeviceAdd(Device device)
        {
            if (ModelState.IsValid)
            {
                // Giriş yapan kullanıcının ID'sini al
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdString, out int userId))
                {
                    // Kullanıcı ID'si başarıyla int olarak alındı
                }
                else
                {
                    // ID'yi alırken hata oluştu
                    // Hata yönetimi yapılabilir (örneğin, hata mesajı veya yönlendirme)
                    return BadRequest();
                }

                device.CreatedDate = DateTime.Now;
                device.UpdatedDate = DateTime.Now;

                // Cihaza kullanıcı ID'sini ata
                device.UserId = userId;

                // Cihaz numarasını otomatik al
                device.DeviceNo = await _deviceService.GenerateNextDeviceNumberAsync(); // Cihaz numarasını al

                // Veritabanına ekle
                _context.Device.Add(device);
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet

                return RedirectToAction("Devices"); // Liste sayfasına yönlendir
            }

            return View(device); // Hata olursa aynı sayfayı göster
        }
        #endregion

        #region EditDevice
        /// <summary>
        /// Urun duzenleme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> EditDevice(int id)
        {
            var device = await _context.Device.FindAsync(id); // Ürünü getir
            if (device == null)
            {
                return NotFound(); // Ürün bulunamazsa hata sayfası
            }
            return View(device);
        }

        [HttpPost]
        public async Task<IActionResult> EditDevice(Device device)
        {
            if (ModelState.IsValid)
            {
                _context.Device.Update(device); // Ürünü güncelle
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
                return RedirectToAction("Devices"); // Liste sayfasına yönlendir
            }
            return View(device); // Hata olursa aynı sayfayı göster
        }

        #endregion

        #region DeleteDevice
        /// <summary>
        /// Urun silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var device = await _context.Device.FindAsync(id); // Ürünü getir
            if (device == null)
            {
                return NotFound(); // Ürün bulunamazsa hata sayfası
            }
            return View(device);
        }

        [HttpPost, ActionName("DeleteDevice")]
        public async Task<IActionResult> DeleteDeviceConfirmed(int id)
        {
            var device = await _context.Device.FindAsync(id); // Ürünü getir
            if (device != null)
            {
                _context.Device.Remove(device); // Ürünü sil
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
            }
            return RedirectToAction("Devices"); // Liste sayfasına yönlendir
        }

        #endregion
    }

}