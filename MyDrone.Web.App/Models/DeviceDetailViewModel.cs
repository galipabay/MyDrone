using MyDrone.Kernel.Models;

namespace MyDrone.Web.App.Models
{
    public class DeviceDetailViewModel
    {
        private Device device;

        public Device Device
        {
            get { return device; }
            set { device = value; }
        }

        private bool isFavorited;

        public bool IsFavorited
        {
            get { return isFavorited; }
            set { isFavorited = value; }
        }

        private bool isSeller;

        public bool IsSeller
        {
            get { return isSeller; }
            set { isSeller = value; }
        }

    }
}