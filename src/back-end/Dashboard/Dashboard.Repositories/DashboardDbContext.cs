using Dashboard.Entitites;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Repositories
{
    public class DashboardDbContext : DbContext
    {
        public DashboardDbContext(DbContextOptions<DashboardDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
