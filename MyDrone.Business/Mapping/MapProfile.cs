using AutoMapper;
using MyDrone.Kernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Business.Mapping
{
	public class MapProfile : Profile
	{
		public MapProfile()
		{
			CreateMap<Device, DeviceDto>();
			CreateMap<Device, DeviceDto>().ReverseMap();

			CreateMap<User, UserDto>();
			CreateMap<User, UserDto>().ReverseMap();
		}
	}
}
