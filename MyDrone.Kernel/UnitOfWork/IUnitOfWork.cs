using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDrone.Kernel.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CommitAsync();// Degisiklikleri kaydet.
        void Commit();
        Task RollbackAsync();
        Task BeginTransactionAsync();// Islem baslatma.
        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);
    }
}
