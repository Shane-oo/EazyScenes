namespace EazyScenes.Data.Entities;

public class User: Entity<UserId>, IAuditableEntity
{
    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? UpdatedOn { get; set; }

    public string UserName { get; set; }

    public string NormalizedUserName { get; set; }

    public string Email { get; set; }

    public string NormalizedEmail { get; set; }

    public DateTimeOffset LoginOn { get; set; }

    public Guid ConcurrencyStamp { get; set; } = Guid.NewGuid();

    public bool LockoutEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public string SecurityStamp { get; set; }

    public int AccessFailedCount { get; set; }
    
    public string Password { get; set; }
    
    public bool EmailConfirmed { get; set; }

    public ICollection<UserClaim> UserClaims { get; set; }

    public UserRole UserRole { get; set; }

    #region Public Methods

    public void AddRole(RoleId roleId)
    {
        UserRole = new UserRole(roleId, Id);
    }

    #endregion
}
