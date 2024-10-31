using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Kernel.Models
{
	public class User : BaseEntity
	{
		#region Propertities

		private string name;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		private string surname;

		public string Surname
		{
			get { return surname; }
			set { surname = value; }
		}

		private string telNo;

		public string TelNo
		{
			get { return telNo; }
			set { telNo = value; }
		}

		private string mailAddress;

		public string MailAddress
		{
			get { return mailAddress; }
			set { mailAddress = value; }
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

		private string apartment;

		public string Apartment
		{
			get { return apartment; }
			set { apartment = value; }
		}

		private bool isSeller;

		public bool IsSeller
		{
			get { return isSeller; }
			set { isSeller = value; }
		}

		private byte[]? image;

		public byte[]? Image
		{
			get { return image; }
			set { image = value; }
		}
		
		#endregion
	}
}
