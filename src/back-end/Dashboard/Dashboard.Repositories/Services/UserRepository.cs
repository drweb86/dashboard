using Dashboard.Entitites;

namespace Dashboard.Repositories.Services
{
    public interface IUserRepository: IBaseRepository<User>
    {

    }

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DashboardDbContext dbContext) : base(dbContext)
        {
        }
    }
}
