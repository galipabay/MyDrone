using Microsoft.EntityFrameworkCore;
using MyDrone.Kernel.Models;
using MyDrone.Kernel.Services;
using MyDrone.Types;

namespace MyDrone.Business.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly AppDbContext _context;

        public DeviceService(AppDbContext context)
        {
            _context = context;
        }

        // GetNextDeviceNo metodunu burada tanımlıyoruz
        public async Task<Int32> GenerateNextDeviceNumberAsync()
        {
            // Veritabanındaki son cihaz numarasını al
            var lastDevice = await _context.Devices.OrderByDescending(d => d.DeviceNo).FirstOrDefaultAsync();

            // Eğer cihaz yoksa, ilk cihaz numarasını 1 yap
            int nextDeviceNo = lastDevice == null ? 1 : lastDevice.DeviceNo + 1;

            // Yeni cihaz numarasını döndür
            return nextDeviceNo;
        }

        public List<Device> SearchDevices(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Device>();

            query = query.ToLower().Trim();
            bool isNumeric = double.TryParse(query, out double numericQuery);

            return _context.Devices
                .Where(d =>
                    (!string.IsNullOrEmpty(d.Brand) && d.Brand.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(d.Model) && d.Model.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(d.Description) && d.Description.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(d.Color) && d.Color.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(d.CamQuality) && d.CamQuality.ToLower().Contains(query)) ||
                    (isNumeric && (
                        d.Range == numericQuery ||
                        d.Price == numericQuery
                    ))
                )
                .ToList();
        }


    }

}
