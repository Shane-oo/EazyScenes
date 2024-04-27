using EazyScenes.Core.Exchange;
using EazyScenes.Data.Entities;

namespace EazyScenes.Users.Authentications;

public class AuthenticateUserWithRefreshTokenCommand: Command
{
    #region Construction

    public AuthenticateUserWithRefreshTokenCommand(UserId userId): base(userId)
    {
    }

    #endregion
}
