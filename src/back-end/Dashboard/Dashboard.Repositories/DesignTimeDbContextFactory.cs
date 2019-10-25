using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.IO;

namespace Dashboard.Repositories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DashboardDbContext>
    {
        public DashboardDbContext CreateDbContext(string[] args)
        {
            Trace.WriteLine(Directory.GetCurrentDirectory());
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),"..", "Dashboard.Api"))
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DashboardDbContext>();
            var connectionString = configuration.GetConnectionString("Postgres");
            builder.UseNpgsql(connectionString);
            return new DashboardDbContext(builder.Options);
        }
    }
}
