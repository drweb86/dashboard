using Dashboard.Entitites;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Repositories
{
    public class DashboardDbContext : DbContext
    {
        public DashboardDbContext(DbContextOptions<DashboardDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Sticker> Stickers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var userMapping = modelBuilder.Entity<User>();
            userMapping.ToTable("Users");
            
            userMapping.Property(e => e.Id).HasColumnName("Id");
            userMapping.HasKey(e => e.Id);

            userMapping.Property(e => e.Username).HasColumnName("Username").IsRequired();
            userMapping.HasIndex(u => u.Username).IsUnique();

            userMapping.Property(e => e.Password).HasColumnName("Password").IsRequired();


            var stickerMapping = modelBuilder.Entity<Sticker>();
            stickerMapping.ToTable("Stickers");

            stickerMapping.Property(e => e.Id).HasColumnName("Id");
            stickerMapping.HasKey(e => e.Id);

            stickerMapping.Property(e => e.Text).HasColumnName("Text");
            stickerMapping.Property(e => e.HtmlColor).HasColumnName("HtmlColor");
            stickerMapping.Property(e => e.X).HasColumnName("X");
            stickerMapping.Property(e => e.Y).HasColumnName("Y");
            stickerMapping.Property(e => e.OwnerId).HasColumnName("OwnerId");
            stickerMapping
                .HasOne(s => s.Owner)
                .WithMany(g => g.Stickers)
                .HasForeignKey(s => s.OwnerId);
        }
    }
}
