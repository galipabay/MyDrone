using MyDrone.Kernel.Models;

namespace MyDrone.Web.App.Models
{
    public class DeviceViewModel
    {
        private Device device;

        public Device Device
        {
            get { return device; }
            set { device = value; }
        }
    }

}
