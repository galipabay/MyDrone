﻿using MyDrone.Kernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Kernel.Services
{
	public interface IService<T> where T : class
	{
		Task<T> GetByIdAsync(int id);
		Task<IEnumerable<T>> GetAllAsync();
		IQueryable<T> Where(Expression<Func<T, bool>> expression);
		Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
		Task<T> AddAsync(T entity);
		Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
		Task UpdateAsync(T entity);
		Task RemoveAsync(T entity);
		Task RemoveRangeAsync(IEnumerable<T> entities);
		List<User> SearchUsers(string query);

	}
}
