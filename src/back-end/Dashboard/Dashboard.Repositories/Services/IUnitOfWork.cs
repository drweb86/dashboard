using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Repositories.Services
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public Task SaveChangesAsync();
    }

    public class UnitOfWork: IUnitOfWork
    {
        private readonly DashboardDbContext _dbContext;
        public UnitOfWork(DashboardDbContext dbContext)
        {
            _dbContext = dbContext;
            UserRepository = new UserRepository(dbContext);
        }

        public IUserRepository UserRepository { get; private set; }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
