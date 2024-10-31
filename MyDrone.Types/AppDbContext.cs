using Microsoft.EntityFrameworkCore;
using MyDrone.Kernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Types
{
	public class AppDbContext : DbContext
	{
		/// <Options>
		/// veri tabani yolunu bu dbcontextoptions uzerinden verecegiz
		/// </Options>
		/// <param name="options"></param>
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
			   
		}

		public DbSet<User> User { get; set; }

		public DbSet<UserDto> UserDtos { get; set; }
		public DbSet<Device> Device { get; set; }

        public DbSet<DeviceDto> DeviceDtos { get; set; }

        //string connectionString = "Data Source=GALIPABAY;Initial Catalog=MyDrone_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        string connectionString = "Data Source=DESKTOP-Q0BKLV1;Initial Catalog=MyDrone_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(connectionString, builder =>
			{
				builder.MigrationsAssembly("MyDrone.Web.App");
			});
		}

		/// <summary>
		/// Model Builder bizim types katmaninda olusturdugumuz configurationlarimizi kaydetmeye yariyor
		/// Assemblyden kaydetmemizin sebebi ise birden cok config olmasi durumunda bize kolaylik saglamasidir-
		/// GetExecutingAssembly de bizim hali hazirda calistigimiz assemblydeki hepsini kaydedecektir.
		/// </summary>
		/// <param name="modelBuilder"></param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(modelBuilder);
		}
	}
}
