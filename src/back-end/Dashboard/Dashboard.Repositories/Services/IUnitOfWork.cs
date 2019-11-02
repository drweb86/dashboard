using System.Threading.Tasks;

namespace Dashboard.Repositories.Services
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IStickerRepository StickerRepository { get; }
        public Task SaveChangesAsync();
    }

    public class UnitOfWork: IUnitOfWork
    {
        private readonly DashboardDbContext _dbContext;
        public UnitOfWork(DashboardDbContext dbContext)
        {
            _dbContext = dbContext;
            UserRepository = new UserRepository(dbContext);
            StickerRepository = new StickerRepository(dbContext);
        }

        public IUserRepository UserRepository { get; private set; }
        public IStickerRepository StickerRepository { get; private set; }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
