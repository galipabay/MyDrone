using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDrone.Kernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Types.Configurations
{
	internal class DeviceConfiguration : IEntityTypeConfiguration<Device>
	{
		public void Configure(EntityTypeBuilder<Device> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).UseIdentityColumn();
			builder.Property(x => x.Street).IsRequired();
			builder.Property(x => x.Province).IsRequired();
			builder.Property(x => x.Altitude).IsRequired();
			builder.Property(x => x.Speed).IsRequired();
			builder.Property(x => x.AirTime).IsRequired();
			builder.Property(x => x.FuelType).IsRequired();
			builder.Property(x => x.Brand).IsRequired();
			builder.Property(x => x.CamQuality).IsRequired();
			builder.Property(x => x.City).IsRequired();
			builder.Property(x => x.Country).IsRequired();
			builder.Property(x => x.Model).IsRequired();
			builder.Property(x => x.Description).IsRequired();
			builder.Property(x => x.District).IsRequired();
			builder.Property(x => x.NightVision).IsRequired();
			builder.Property(x => x.Range).IsRequired();

			builder.ToTable("Device");
		}
	}
}
