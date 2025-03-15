using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Kernel.Models
{
    public class DeviceAttributes
    {
		#region Propertities

		
		private int id;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		private string attributeType;

		public string AttributeType
		{
			get { return attributeType; }
			set { attributeType = value; }
		}

		private string attributeValue;

		public string AttributeValue
        {
			get { return attributeValue; }
			set { attributeValue = value; }
		}

		private int deviceChainNo;

		public int DeviceChainNo
        {
			get { return deviceChainNo; }
			set { deviceChainNo = value; }
		}

        #endregion
    }
}
