using Dashboard.Entitites;
using System;
using System.Collections.Generic;
using System.Text;

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
