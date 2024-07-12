using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Kernel.Models
{
	public class Device : BaseEntity
	{

		#region Propertities

		private int deviceNo;

		public int DeviceNo
		{
			get { return deviceNo; }
			set { deviceNo = value; }
		}

		private string brand;

		public string Brand
		{
			get { return brand; }
			set { brand = value; }
		}

		private string model;

		public string Model
		{
			get { return model; }
			set { model = value; }
		}

		private int speed;

		public int Speed
		{
			get { return speed; }
			set { speed = value; }
		}

		private int airTime;

		public int AirTime
		{
			get { return airTime; }
			set { airTime = value; }
		}

		private string camQuality;

		public string CamQuality
		{
			get { return camQuality; }
			set { camQuality = value; }
		}

		private bool nightVision;

		public bool NightVision
		{
			get { return nightVision; }
			set { nightVision = value; }
		}

		private int altitude;

		public int Altitude
		{
			get { return altitude; }
			set { altitude = value; }
		}

		private string color;

		public string Color
		{
			get { return color; }
			set { color = value; }
		}

		private int range;

		public int Range
		{
			get { return range; }
			set { range = value; }
		}

		private short fuelType;

		public short FuelType
		{
			get { return fuelType; }
			set { fuelType = value; }
		}

		private string description;

		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		private byte[] image;

		public byte[] Image
		{
			get { return image; }
			set { image = value; }
		}

		private string video;

		public string Video
		{
			get { return video; }
			set { video = value; }
		}

		private string country;

		public string Country
		{
			get { return country; }
			set { country = value; }
		}

		private string city;

		public string City
		{
			get { return city; }
			set { city = value; }
		}

		private string province;

		public string Province
		{
			get { return province; }
			set { province = value; }
		}

		private string district;

		public string District
		{
			get { return district; }
			set { district = value; }
		}

		private string street;

		public string Street
		{
			get { return street; }
			set { street = value; }
		}

		#endregion
	}
}