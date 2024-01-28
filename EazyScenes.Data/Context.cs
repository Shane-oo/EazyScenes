using EazyScenes.Data.Entities;
using EazyScenes.Data.Entities.AuthorizationEntities;
using Microsoft.EntityFrameworkCore;

namespace EazyScenes.Data;

public class Context: DbContext
{
    #region Properties

    public DbSet<Application> Applications { get; set; }

    public DbSet<Authorization> Authorizations { get; set; }

    public DbSet<RoleClaim> RoleClaims { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Scope> Scopes { get; set; }

    public DbSet<Token> Tokens { get; set; }

    public DbSet<UserClaim> UserClaims { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<User> Users { get; set; }

    #endregion

    #region Construction

    public Context(DbContextOptions options): base(options)
    {
    }

    #endregion

    #region Private Methods

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Apply all configurations in EazyScenes.Data
        builder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
    }

    #endregion
}
