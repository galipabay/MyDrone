using Microsoft.EntityFrameworkCore;
using MyDrone.Kernel.Services;
using MyDrone.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Business.Services
{
    public class DeviceService :IDeviceService
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
            var lastDevice = await _context.Device.OrderByDescending(d => d.DeviceNo).FirstOrDefaultAsync();

            // Eğer cihaz yoksa, ilk cihaz numarasını 1 yap
            int nextDeviceNo = lastDevice == null ? 1 : lastDevice.DeviceNo + 1;

            // Yeni cihaz numarasını döndür
            return nextDeviceNo;
        }
    }

}
