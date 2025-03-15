using MyDrone.Kernel.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MyDrone.Kernel.Models;
using BCrypt.Net;

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
		public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
		{
			return await _context.Set<T>().ToListAsync();
		}
		public async Task<User?> AuthenticateAsync(string identifier, string password)
		{
			var user = await _context.User
				.AsNoTracking()
				.SingleOrDefaultAsync(x => x.MailAddress == identifier || x.TelNo == identifier);

			if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
			{
				await Task.Delay(300); // Brute-force saldırılarına karşı gecikme ekleyelim
				return null;
			}

			return user;
		}

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

		// Soft delete işlemi
		public async Task SoftDeleteAsync(T entity)
		{
			//galip
			//var entity = await _dbSet.FindAsync(id);
			if (entity != null)
			{
				// IsDeleted flag'ını true yapıyoruz
				_context.Entry(entity).Property("IsDeleted").CurrentValue = true;
				// Silinme tarihini kaydediyoruz
				_context.Entry(entity).Property("DeletedDate").CurrentValue = DateTime.UtcNow;
				await _context.SaveChangesAsync();
			}
		}
		public async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
		{
			return await _dbSet.Skip((pageNumber - 1) * pageSize)
							   .Take(pageSize)
							   .AsNoTracking()
							   .ToListAsync();
		}
	}
}
