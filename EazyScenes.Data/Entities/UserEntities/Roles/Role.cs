namespace EazyScenes.Data.Entities;

public class Role: Entity<RoleId>
{
    public string Name { get; set; }

    public string NormalizedName { get; set; }

    public Guid ConcurrencyStamp { get; set; } = Guid.NewGuid();

    public virtual ICollection<RoleClaim> RoleClaims { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }
}
