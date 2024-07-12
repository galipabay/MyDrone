using AutoMapper;
using DMBD.Api.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyDrone.Api.Controllers;
using MyDrone.Kernel.DTOs;
using MyDrone.Kernel.Models;
using MyDrone.Kernel.Services;

namespace MyDrone.Web.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : CustomBaseController
	{
		private readonly IMapper _mapper;
		private readonly IService<User> _service;

		public UserController(IMapper mapper, IService<User> service)
		{
			_mapper = mapper;
			_service = service;
		}

		// get api/user
		[HttpGet]
		public async Task<IActionResult> All()
		{
			var users = await _service.GetAllAsync();

			var usersDto = _mapper.Map<List<UserDto>>(users.ToList());
			return CreateActionResult(CustomResponseDto<List<UserDto>>.Success(200, usersDto));
		}

		[ServiceFilter(typeof(NotFoundFilter<User>))]
		// GET /api/user/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{


			var user = await _service.GetByIdAsync(id);
			var userDto = _mapper.Map<UserDto>(user);
			return CreateActionResult(CustomResponseDto<UserDto>.Success(200, userDto));
		}

		[HttpPost]
		public async Task<IActionResult> Save(UserDto userDto)
		{
			var user = await _service.AddAsync(_mapper.Map<User>(userDto));
			var usersDto = _mapper.Map<UserDto>(user);
			return CreateActionResult(CustomResponseDto<UserDto>.Success(201, usersDto));
		}

		
		[HttpPut]
		public async Task<IActionResult> Update(UserDto userDto)
		{
			await _service.UpdateAsync(_mapper.Map<User>(userDto));

			return CreateActionResult(CustomResponseDto<NoContent>.Success(204));
		}

		// DELETE api/user/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Remove(int id)
		{
			var user = await _service.GetByIdAsync(id);

			await _service.RemoveAsync(user);

			return CreateActionResult(CustomResponseDto<NoContent>.Success(204));
		}
	}
}
