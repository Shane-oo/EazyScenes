using EazyScenes.Core;
using EazyScenes.Core.Exchange;
using EazyScenes.Data;
using EazyScenes.Data.Entities;
using EazyScenes.Data.Entities.Queries;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace EazyScenes.Users.Authentications;

[UsedImplicitly]
public class AuthenticateUserWithRefreshTokenHandler: ICommandHandler<AuthenticateUserWithRefreshTokenCommand>
{
    #region Fields

    private readonly IDataContext _dataContext;
    private readonly UserManager<User> _userManager;

    #endregion

    #region Construction

    public AuthenticateUserWithRefreshTokenHandler(IDataContext dataContext, UserManager<User> userManager)
    {
        _dataContext = dataContext;
        _userManager = userManager;
    }

    #endregion

    #region Public Methods

    public async Task<Result> Handle(AuthenticateUserWithRefreshTokenCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var user = await _dataContext.Query<IUserByUserIdDbQuery>()
                                     .WithParams(command.UserId)
                                     .ExecuteAsync(cancellationToken);

        if (user == null)
        {
            return Result.Failure<UserAuthenticationPayload>(Error.NotFound(nameof(User)));
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            return Result.Failure<UserAuthenticationPayload>(new Error("User.LockedOut", "User has been locked out, Reset Password Required"));
        }

        user.LoginOn = DateTimeOffset.UtcNow;

        await _dataContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    #endregion
}
