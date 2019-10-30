using Dashboard.Entitites;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Repositories
{
    public class DashboardDbContext : DbContext
    {
        public DashboardDbContext(DbContextOptions<DashboardDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var userMapping = modelBuilder.Entity<User>();
            userMapping.ToTable("Users");
            
            userMapping.Property(e => e.Id).HasColumnName("Id");
            userMapping.HasKey(e => e.Id);

            userMapping.Property(e => e.FirstName).HasColumnName("FirstName").IsRequired();
            userMapping.Property(e => e.LastName).HasColumnName("LastName").IsRequired();

            userMapping.Property(e => e.Username).HasColumnName("Username").IsRequired();
            userMapping.HasIndex(u => u.Username).IsUnique();

            userMapping.Property(e => e.Password).HasColumnName("Password").IsRequired();
        }
    }
}
