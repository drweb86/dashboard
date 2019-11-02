using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dashboard.Repositories.Services
{
    public interface IBaseRepository<TEntity>
    {
        void Add(TEntity entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> filter);
        Task<bool> UpdateAsync(int id, TEntity entityUpdate);
        Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> filter, TEntity entityUpdate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);
    }

    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity: class
    {
        protected DashboardDbContext Db { get; private set; }
        protected BaseRepository(DashboardDbContext dbContext)
        {
            Db = dbContext;
        }

        public void Add(TEntity entity)
        {
            Db.Set<TEntity>().Add(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await Db.Set<TEntity>().FindAsync(id);
            if (entity == null)
                return false;

            Db.Set<TEntity>().Remove(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            var entity = await Db.Set<TEntity>()
                .SingleOrDefaultAsync(filter);

            if (entity == null)
                return false;

            Db.Set<TEntity>().Remove(entity);
            return true;
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Db.Set<TEntity>()
                .AsNoTracking()
                .Where(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> filter, TEntity entityUpdate)
        {
            var entity = await Db.Set<TEntity>()
                .SingleOrDefaultAsync(filter);

            if (entity == null)
                return false;

            Db.Entry(entity).CurrentValues.SetValues(entityUpdate);
            return true;
        }

        public async Task<bool> UpdateAsync(int id, TEntity entityUpdate)
        {
            var entity = await Db.Set<TEntity>().FindAsync(id);
            if (entity == null)
                return false;

            Db.Entry(entity).CurrentValues.SetValues(entityUpdate);
            return true;
        }
    }
}
