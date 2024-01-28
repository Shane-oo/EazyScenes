namespace EazyScenes.Data.Entities;

public class UserClaim: Entity<UserClaimId>
{
    public string ClaimType { get; set; }

    public string ClaimValue { get; set; }

    public UserId UserId { get; set; }

    public User User { get; set; }
}
