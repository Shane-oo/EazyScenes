using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EazyScenes.Data.Entities.Configurations;

public class UserRoleConfiguration: IEntityTypeConfiguration<UserRole>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(e => new { e.UserId, e.RoleId });

        // Each User can only have one role
        builder.HasOne(e => e.User)
               .WithOne(u => u.UserRole)
               .HasForeignKey<UserRole>(e => e.UserId)
               .IsRequired();

        // Each Role can have many entries in the UserRole join table
        builder.HasOne(e => e.Role)
               .WithMany(r => r.UserRoles)
               .HasForeignKey(e => e.RoleId)
               .IsRequired();

        builder.HasIndex(e => new { e.UserId, e.RoleId })
               .IsUnique();
    }

    #endregion
}
