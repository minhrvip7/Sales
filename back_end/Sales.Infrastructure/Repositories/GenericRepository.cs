using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Sales.Domain.Interfaces;
using Sales.Domain.IRepositories;
using Sales.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Sales.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly SalesDbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public GenericRepository(SalesDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetQueryable() => DbSet;

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.CountAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int offset = 0,
            int limit = -1,
            bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = DbSet;

            // Bỏ qua Global Query Filter (ví dụ: IsDeleted) khi cần xem lịch sử/báo cáo
            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (offset > 0)
            {
                query = query.Skip(offset);
            }

            if (limit > 0)
            {
                query = query.Take(limit);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "",
            bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = DbSet;

            // Bỏ qua Global Query Filter khi cần xem lịch sử/báo cáo
            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            return (await query.FirstOrDefaultAsync())!;
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public virtual async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public virtual async Task DeleteAsync(object id)
        {
            TEntity entityToDelete = (await DbSet.FindAsync(id))!;
            if (entityToDelete != null)
            {
                Delete(entityToDelete);
            }
        }

        /// <summary>
        /// Xóa mềm nếu entity implement ISoftDelete, ngược lại xóa vật lý.
        /// </summary>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (entityToDelete is ISoftDelete softEntity)
            {
                // Soft delete: chỉ đánh dấu IsDeleted = true, không xóa thật
                softEntity.IsDeleted = true;
                softEntity.DeletedDate = DateTime.UtcNow;
                Update(entityToDelete);
            }
            else
            {
                // Hard delete fallback cho entity không implement ISoftDelete
                if (Context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    DbSet.Attach(entityToDelete);
                }
                DbSet.Remove(entityToDelete);
            }
        }

        /// <summary>
        /// Xóa mềm nhiều entity cùng lúc nếu implement ISoftDelete.
        /// </summary>
        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }
    }
}
