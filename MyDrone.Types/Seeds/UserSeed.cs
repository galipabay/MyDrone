using MyDrone.Kernel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Types.Seed
{
    internal class UserSeed : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.HasData(new User 
            { 
                Id = 1,
                Name = "Galip",
                Surname = "Abay",
                TelNo = "sadasdasada",
                MailAddress = "w31312",
                Country = "1234567890",
                City = "1234567890",
                Province = "1234567890",
                Apartment = "1234567890",
                District = "1234567890",
                Street = "1234567890",
                IsSeller = true,
                CreatedDate = DateTime.Now,
            });
        }
    }
}
