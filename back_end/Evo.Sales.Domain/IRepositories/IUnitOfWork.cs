using System;
using System.Threading.Tasks;

namespace Evo.Sales.Domain.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync();
    }
}
