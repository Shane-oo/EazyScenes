namespace EazyScenes.Data.Entities;

public class RoleClaim: Entity<RoleClaimId>
{
    public RoleId RoleId { get; set; }

    public Role Role { get; set; }

    public string ClaimType { get; set; }

    public virtual string ClaimValue { get; set; }
}
