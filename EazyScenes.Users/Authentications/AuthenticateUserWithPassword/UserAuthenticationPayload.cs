using EazyScenes.Data.Entities;

namespace EazyScenes.Users.Authentications.AuthenticateUserWithPassword;

public class UserAuthenticationPayload
{
    #region Properties

    public string Role { get; init; }

    public UserId UserId { get; init; }

    public string UserName { get; init; }

    #endregion
}
