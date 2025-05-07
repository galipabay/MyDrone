using MyDrone.Kernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyDrone.Kernel.Services
{
    public interface IDeviceService
    {
        Task<Int32> GenerateNextDeviceNumberAsync();
        List<Device> SearchDevices(string query);
    }
}
