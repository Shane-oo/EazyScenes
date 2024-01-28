using System.Reflection;
using EazyScenes.Core;
using EazyScenes.Data;
using EazyScenes.Data.Entities;
using EazyScenes.Data.Entities.AuthorizationEntities;
using EazyScenes.Data.Entities.Queries;
using EazyScenes.Users.Users;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

namespace EazyScenes.Web;

public static class DependencyInjection
{
    #region Private Methods

    private static Assembly[] GetAssemblies()
    {
        return
        [
            typeof(DependencyInjection).Assembly, // EazyScenes.Web
            typeof(UsersModule).Assembly // EazyScenes.Users
        ];
    }

    #endregion

    #region Public Methods

    public static void AddDataContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Context>((sp, o) =>
                                       {
                                           var connectionString = configuration.GetConnectionString("eazy-scenes-db");
                                           ArgumentException.ThrowIfNullOrEmpty(connectionString);

                                           o.UseSqlServer(connectionString);

                                           o.UseOpenIddict<Application, Authorization, Scope, Token, int>();
                                       });

        services.AddScoped<IDataContext, DataContext>();
    }

    public static void AddDbQueries(this IServiceCollection services)
    {
        services.AddTransient<IUserByUserIdDbQuery, UserByUserIdDbQuery>();
        services.AddTransient<IUserByNameDbQuery, UserByNameDbQuery>();
        services.AddTransient<IUserByEmailDbQuery, UserByEmailDbQuery>();
        services.AddTransient<IUsersByRoleNamesDbQuery, UsersByRoleNamesDbQuery>();
        services.AddTransient<IRoleByRoleIdDbQuery, RoleByRoleIdDbQuery>();
        services.AddTransient<IRoleByNameDbQuery, RoleByNameDbQuery>();
        
    }

    public static void AddFluentValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblies(GetAssemblies());
    }

    public static void AddMediatRServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
                            {
                                config.RegisterServicesFromAssemblies(GetAssemblies());

                                config.AddOpenBehavior(typeof(FluentValidationBehaviour<,>));
                            });
    }

    public static void AddOpenIddictServer(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddOpenIddict()
                .AddCore(o =>
                         {
                             o.UseEntityFrameworkCore()
                              .UseDbContext<Context>()
                              .ReplaceDefaultEntities<Application, Authorization, Scope, Token, int>();
                         })
                .AddServer(o =>
                           {
                               
                               o.SetAccessTokenLifetime(TimeSpan.FromMinutes(120))
                                .SetRefreshTokenLifetime(TimeSpan.FromDays(14))
                                .SetIdentityTokenLifetime(TimeSpan.FromMinutes(120));

                               o.AllowPasswordFlow();

                               o.SetTokenEndpointUris("api/authentications/token");

                               if (environment.IsDevelopment())
                               {
                                   o.AddDevelopmentEncryptionCertificate()
                                    .AddDevelopmentSigningCertificate();
                               }

                               //else {o.AddEncryptionCertificate(LoadCertificate(...)
                               // .AddSigningCertificate(LoadCertificate(...)}
                               o.UseAspNetCore()
                                .EnableTokenEndpointPassthrough();
                           })
                .AddValidation(o =>
                               {
                                   o.UseLocalServer();
                                   o.UseAspNetCore();
                               });
    }


    public static void AddUserIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(o =>
                                         {
                                             o.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
                                             o.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                                             o.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;

                                             o.User.RequireUniqueEmail = true;
                                         })
                .AddUserManager<UserManager<User>>()
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>()
                .AddDefaultTokenProviders();
    }

    #endregion
}
