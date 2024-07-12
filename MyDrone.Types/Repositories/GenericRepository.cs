using MyDrone.Kernel.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Types.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		protected readonly AppDbContext _context;
		private readonly DbSet<T> _dbSet;

		public GenericRepository(AppDbContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}

		public async Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
		}

		public async Task AddRangeAsync(IEnumerable<T> entities)
		{
			await _dbSet.AddRangeAsync(entities);
		}
		public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
		{
			return await _dbSet.AnyAsync(expression);
		}

		//public Task AuthenticateAsync(T entity,)
		//{
		//	// Şifreyi hashleyip karşılaştırmak daha güvenli bir yaklaşım olur
		//	var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username && x.Password == password);

		//	if (user == null)
		//	{
		//		return null;
		//	}

		//	return user;
		//}

		/// <summary>
		/// AsNoTracking ef core cektigi datalari memory'e almayip ve daha optimize calismasini saglar.
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		public IQueryable<T> GetAll()
		{
			return _dbSet.AsNoTracking().AsQueryable();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public void Remove(T entity)
		{
			_dbSet.Remove(entity);
		}
		public void RemoveRange(IEnumerable<T> entities)
		{
			_dbSet.RemoveRange(entities);
		}

		public void Update(T entity)
		{
			_dbSet.Update(entity);
		}

		public IQueryable<T> Where(Expression<Func<T, bool>> expression)
		{
			return _dbSet.Where(expression);
		}
	}
}
