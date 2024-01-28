using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EazyScenes.Data.Entities.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasConversion(id => id.Value, value => new UserId(value))
               .ValueGeneratedOnAdd();

        builder.Property(e => e.UserName)
               .HasMaxLength(31);

        builder.Property(e => e.NormalizedUserName)
               .HasMaxLength(31);

        builder.Property(e => e.ConcurrencyStamp)
               .IsConcurrencyToken();

        builder.HasIndex(e => e.NormalizedUserName)
               .IsUnique();
    }
}
