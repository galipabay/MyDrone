using Microsoft.EntityFrameworkCore;
using MyDrone.Kernel.Models;
using MyDrone.Kernel.Repository;
using MyDrone.Kernel.Services;
using MyDrone.Kernel.UnitOfWork;
using MyDrone.Types;
using System.Linq.Expressions;

namespace MyDrone.Business.Services
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        public Service(IGenericRepository<T> repository, IUnitOfWork unitOfWork, AppDbContext context)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            return entities;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _repository.AnyAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new Exception($"{typeof(T).Name} with ID {id} not found.");
            }
            return entity;
        }

        public async Task RemoveAsync(T entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _repository.Where(expression);
        }

        public async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _repository.GetPagedAsync(pageNumber, pageSize);
        }
        public async Task<int> GetCountAsync()
        {
            return await _repository.GetAll().CountAsync();
        }
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression)
        {
            return await _repository.Where(expression).FirstOrDefaultAsync();
        }
        public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return await _unitOfWork.ExecuteSqlRawAsync(sql, parameters);
        }
        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await action();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
        public async Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> expression)
        {
            return await _repository.Where(expression).ToListAsync();
        }
        public async Task SoftDeleteAsync(int id)
        {
            // Öncelikle entity'yi repository üzerinden alın
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                // BaseEntity'deki IsDeleted ve DeletedDate alanlarına erişim sağlanır
                if (entity is BaseEntity baseEntity)
                {
                    baseEntity.IsDeleted = true;
                    baseEntity.DeletedDate = DateTime.UtcNow;
                    _repository.Update(entity); // Entity'yi güncelle
                    await _unitOfWork.CommitAsync(); // Değişiklikleri kaydet
                }
                else
                {
                    throw new Exception($"Entity does not implement {nameof(BaseEntity)}.");
                }
            }
            else
            {
                throw new Exception($"{typeof(T).Name} with ID {id} not found.");
            }
        }

        public List<User> SearchUsers(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<User>();

            query = query.ToLower().Trim();

            return _context.Users
                .Where(u =>
                    (!string.IsNullOrEmpty(u.Name) && u.Name.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(u.Surname) && u.Surname.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(u.TelNo) && u.TelNo.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(u.MailAddress) && u.MailAddress.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(u.Country) && u.Country.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(u.City) && u.City.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(u.Province) && u.Province.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(u.District) && u.District.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(u.Street) && u.Street.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(u.Apartment) && u.Apartment.ToLower().Contains(query))
                )
                .ToList();
        }

    }
}