using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Kernel.Models
{
	public class Favorite : BaseEntity
	{
		private int userId;

		public int UserId
		{
			get { return userId; }
			set { userId = value; }
		}

		private int deviceId;

		public int DeviceId
		{
			get { return deviceId; }
			set { deviceId = value; }
		}

		private User user;

		public User User
		{
			get { return user; }
			set { user = value; }
		}

		private Device device;

		public Device Device
		{
			get { return device; }
			set { device = value; }
		}
	}
}
