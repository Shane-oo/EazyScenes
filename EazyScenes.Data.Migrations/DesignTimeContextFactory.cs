using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EazyScenes.Data.Migrations;

[UsedImplicitly]
public class DesignTimeContextFactory: IDesignTimeDbContextFactory<Context>
{
    #region Public Methods

    public Context CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("local.settings.json", true, true)
                            .AddEnvironmentVariables()
                            .Build();

        var connectionString = configuration.GetConnectionString("eazy-scenes-db");

        ArgumentException.ThrowIfNullOrEmpty(connectionString);

        var builder = new DbContextOptionsBuilder<Context>()
            .UseSqlServer(connectionString, b => { b.MigrationsAssembly(GetType().Assembly.GetName().Name); });

        return new Context(builder.Options);
    }

    #endregion
}
