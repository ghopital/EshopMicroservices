using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ordering.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) //Current directory from the startup project in this case Ordering.API project
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional:true)
            .Build();

        string connectionString = configuration.GetConnectionString("Database");

        var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionBuilder.UseSqlServer(connectionString);
        return new ApplicationDbContext(optionBuilder.Options);
    }
}
