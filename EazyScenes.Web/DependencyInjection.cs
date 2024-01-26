using EazyScenes.Core;
using EazyScenes.Data;
using Microsoft.EntityFrameworkCore;

namespace EazyScenes.Web;

public static class DependencyInjection
{
    #region Public Methods

    public static void AddDataContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Context>((sp, o) =>
                                       {
                                           var connectionString = configuration.GetConnectionString("eazy-scenes-db");
                                           ArgumentException.ThrowIfNullOrEmpty(connectionString);

                                           o.UseSqlServer(connectionString);
                                       });
    }

    public static void AddMediatRServices(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        services.AddMediatR(config =>
                            {
                                config.RegisterServicesFromAssemblies(assembly); // todo will probably need assemblies from other projects

                                config.AddOpenBehavior(typeof(FluentValidationBehaviour<,>));
                            });
    }

    #endregion
}
