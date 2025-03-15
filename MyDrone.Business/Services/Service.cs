using MyDrone.Kernel.Repository;
using MyDrone.Kernel.Services;
using MyDrone.Kernel.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using MyDrone.Kernel.Models;

namespace MyDrone.Business.Services
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public Service(IGenericRepository<T> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
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


    }
}