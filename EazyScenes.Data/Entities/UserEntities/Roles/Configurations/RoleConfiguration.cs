using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EazyScenes.Data.Entities.Configurations;

public class RoleConfiguration: IEntityTypeConfiguration<Role>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasConversion(id => id.Value, value => new RoleId(value))
               .ValueGeneratedOnAdd();

        builder.Property(e => e.ConcurrencyStamp).IsConcurrencyToken();
        builder.Property(e => e.Name).HasMaxLength(256);
        builder.Property(e => e.NormalizedName).HasMaxLength(256);

        builder.HasIndex(e => e.NormalizedName)
               .IsUnique();
    }

    #endregion
}
