using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Be.Data.Data
{
    public class DesignTimeIdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            return Create(Directory.GetCurrentDirectory(),
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        }

        public IdentityDbContext Create()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var basePath = AppContext.BaseDirectory;
            return Create(basePath, environmentName);
        }

        public IdentityDbContext Create(string basePath, string environmentName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            var connectionString = config.GetConnectionString("Default");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Could not find a connection string name Default");
            }

            return Create(connectionString);
        }

        private IdentityDbContext Create(string connectionString)
        {            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new AggregateException(connectionString);
            }

            var optionBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
            optionBuilder.UseNpgsql(connectionString);
            var options = optionBuilder.Options;
            return new IdentityDbContext(options);
        }
    }
}
