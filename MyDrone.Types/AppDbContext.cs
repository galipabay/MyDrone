using Microsoft.EntityFrameworkCore;
using MyDrone.Kernel.Models;
using System.Reflection;

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

        public DbSet<User> Users { get; set; }
        public DbSet<UserDto> UserDtos { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceDto> DeviceDtos { get; set; }
        public DbSet<DeviceAttributes> DeviceAttributes { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<RecentlyViewed> RecentlyViewed { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Message> Messages { get; set; }

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

            // User entity'si için yapılandırma
            modelBuilder.Entity<User>()
                .Property(u => u.IsDeleted)
                .HasDefaultValue(false);  // Varsayılan olarak false (silinmedi)

            modelBuilder.Entity<User>()
                .Property(u => u.DeletedDate)
                .IsRequired(false);  // Opsiyonel, ancak null olabilir

            // Device entity'si için yapılandırma
            modelBuilder.Entity<Device>()
                .Property(d => d.IsDeleted)
                .HasDefaultValue(false);  // Varsayılan olarak false (silinmedi)

            modelBuilder.Entity<Device>()
                .Property(d => d.DeletedDate)
                .IsRequired(false);  // Opsiyonel, ancak null olabilir
        }
    }
}
