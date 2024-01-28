using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EazyScenes.Data.Entities.AuthorizationEntities.Configuration;

public class ScopeConfiguration: IEntityTypeConfiguration<Scope>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Scope> builder)
    {
        builder.ToTable("Scopes");

        builder.Property(e => e.ConcurrencyToken).HasMaxLength(50)
               .IsConcurrencyToken();
        builder.Property(e => e.Name).HasMaxLength(200);

        builder.HasIndex(e => e.Name)
               .IsUnique();
    }

    #endregion
}
