﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Kernel.Models
{
	public class BaseEntityDto
	{
		private int id;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		private DateTime createdDate;

		public DateTime CreatedDate
		{
			get { return createdDate; }
			set { createdDate = value; }
		}

	}
}
