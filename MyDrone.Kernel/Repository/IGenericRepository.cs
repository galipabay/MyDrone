using MyDrone.Kernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Kernel.Repository
{
	public interface IGenericRepository<T> where T : class
	{
		Task<T> GetByIdAsync(int id);
		IQueryable<T> GetAll();
		IQueryable<T> Where(Expression<Func<T, bool>> expression);
		Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
		Task AddAsync(T entity);
		Task AddRangeAsync(IEnumerable<T> entities);
		void Update(T entity);
		void Remove(T entity);
		void RemoveRange(IEnumerable<T> entities);
        Task<User?> AuthenticateAsync(string identifier, string password);
        Task SoftDeleteAsync(T entity);
		Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
	}
}
