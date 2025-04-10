using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Kernel.Models
{
	public class BaseEntity
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

		private DateTime? updatedDate;

		public DateTime? UpdatedDate
		{
			get { return updatedDate; }
			set { updatedDate = value; }
		}

		// Yumuşak silme için eklenen alan
		private bool isDeleted;

		public bool IsDeleted
		{
			get { return isDeleted; }
			set { isDeleted = value; }
		}

		private DateTime? deletedDate;

		public DateTime? DeletedDate
		{
			get { return deletedDate; }
			set { deletedDate = value; }
		}

		private string? deletedBy;

		public string? DeletedBy
		{
			get { return deletedBy; }
			set { deletedBy = value; }
		}

		private string? deleteReason;

		public string? DeleteReason
		{
			get { return deleteReason; }
			set { deleteReason = value; }
		}

	}
}
