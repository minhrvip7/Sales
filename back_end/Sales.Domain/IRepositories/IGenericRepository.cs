using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sales.Domain.IRepositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null);

        IQueryable<TEntity> GetQueryable();

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int offset = 0,
            int limit = -1,
            bool ignoreQueryFilters = false);

        Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "",
            bool ignoreQueryFilters = false);

        Task InsertAsync(TEntity entity);
        Task InsertRangeAsync(IEnumerable<TEntity> entities);
        void Update(TEntity entityToUpdate);
        void UpdateRange(IEnumerable<TEntity> entities);
        Task DeleteAsync(object id);
        void Delete(TEntity entityToDelete);
        void DeleteRange(IEnumerable<TEntity> entities);
    }
}
