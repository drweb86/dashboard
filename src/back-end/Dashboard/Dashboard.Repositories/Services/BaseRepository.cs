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

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Db.Set<TEntity>()
                .AsNoTracking()
                .Where(filter)
                .FirstOrDefaultAsync();
        }
    }
}
