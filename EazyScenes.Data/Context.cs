using Microsoft.EntityFrameworkCore;

namespace EazyScenes.Data;

public class Context: DbContext
{
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
