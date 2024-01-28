using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EazyScenes.Data.Entities.Configurations;

public class UserClaimConfiguration: IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.ToTable("UserClaims");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasConversion(id => id.Value, value => new UserClaimId(value))
               .ValueGeneratedOnAdd();
        
        // Each User can have many UserClaims
        builder.HasOne(e => e.User)
               .WithMany(u => u.UserClaims)
               .HasForeignKey(e => e.UserId)
               .IsRequired();
    }
}
