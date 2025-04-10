using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Kernel.Models
{
    public class RecentlyViewed
    {
		private int id;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}
        // Kullanıcı ID'si
        private int userId;

		public int UserId
		{
			get { return userId; }
			set { userId = value; }
		}
        // Görüntülenen cihaz ID'si
        private int deviceId;

		public int DeviceId
		{
			get { return deviceId; }
			set { deviceId = value; }
		}
        // Görüntülenme zamanı
        private DateTime viewedAt;

		public DateTime ViewedAt
		{
			get { return viewedAt; }
			set { viewedAt = value; }
		}

	}
}
