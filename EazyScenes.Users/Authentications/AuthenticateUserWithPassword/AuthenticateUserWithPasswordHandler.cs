using EazyScenes.Core;
using EazyScenes.Core.Exchange;
using EazyScenes.Data;
using EazyScenes.Data.Entities;
using EazyScenes.Data.Entities.Queries;
using Microsoft.AspNetCore.Identity;

namespace EazyScenes.Users.Authentications.AuthenticateUserWithPassword;

public class AuthenticateUserWithPasswordHandler: ICommandHandler<AuthenticateUserWithPasswordCommand, UserAuthenticationPayload>
{
    #region Fields

    private readonly IDataContext _dataContext;
    private readonly UserManager<User> _userManager;

    #endregion

    #region Construction

    public AuthenticateUserWithPasswordHandler(IDataContext dataContext, UserManager<User> userManager)
    {
        _dataContext = dataContext;
        _userManager = userManager;
    }

    #endregion

    #region Public Methods

    public async Task<Result<UserAuthenticationPayload>> Handle(AuthenticateUserWithPasswordCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        
        var user = await _dataContext.Query<IUserByNameDbQuery>()
                                     .Include($"{nameof(User.UserRole)}.{nameof(UserRole.Role)}")
                                     .WithParams(command.UserName)
                                     .ExecuteAsync(cancellationToken);

        if (user == null)
        {
            return Result.Failure<UserAuthenticationPayload>(Error.NotFound(nameof(User)));
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            return Result.Failure<UserAuthenticationPayload>(new Error("User.LockedOut", "User has been locked out, Reset Password Required"));
        }
        
        // if want to make sure email is confirmed before logging
        //if(await _userManager.IsEmailConfirmedAsync(user))
            //...

        if (!await _userManager.CheckPasswordAsync(user, command.Password))
        {
            await _userManager.AccessFailedAsync(user);
            return Result.Failure<UserAuthenticationPayload>(Error.NotFound(nameof(User)));
        }

        await _userManager.ResetAccessFailedCountAsync(user);
        
        return new UserAuthenticationPayload
               {
                   UserId = user.Id,
                   UserName = user.UserName,
                   Role = user.UserRole.Role.Name
               };
    }

    #endregion
}
