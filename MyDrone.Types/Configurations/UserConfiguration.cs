using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDrone.Kernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Types.Configurations
{
	internal class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).UseIdentityColumn();
			builder.Property(x => x.Name).IsRequired();
			builder.Property(x => x.Surname).IsRequired();
			builder.Property(x => x.Street).IsRequired();
			builder.Property(x => x.Province).IsRequired();
			builder.Property(x => x.IsSeller).IsRequired();
			builder.Property(x => x.Apartment).IsRequired();
			builder.Property(x => x.City).IsRequired();
			builder.Property(x => x.Country).IsRequired();
			builder.Property(x => x.District).IsRequired();
			builder.Property(x => x.MailAddress).IsRequired();
			builder.Property(x => x.TelNo).IsRequired();

			builder.ToTable("User");
		}
	}
}
