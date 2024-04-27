using EazyScenes.Core;
using EazyScenes.Core.Exchange;
using EazyScenes.Data.Entities;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

namespace EazyScenes.Users.Users.CreateUser;

[UsedImplicitly]
public class CreateUserHandler: ICommandHandler<CreateUserCommand>
{
    #region Fields

    private readonly UserManager<User> _userManager;

    #endregion

    #region Construction

    public CreateUserHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    #endregion

    #region Public Methods

    public async Task<Result> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var user = new User
                   {
                       Email = command.Email,
                       UserName = command.UserName,
                       CreatedOn = DateTimeOffset.UtcNow // todo implement entity framework core interceptor
                   };
        var userCreateResult = await _userManager.CreateAsync(user, command.Password);

        if (!userCreateResult.Succeeded)
        {
            var identityError = userCreateResult.Errors.FirstOrDefault();
            return Result.Failure(identityError != null ? new Error(identityError.Code, identityError.Description) : Error.Unknown);
        }

        var roleAddResult = await _userManager.AddToRoleAsync(user, nameof(Roles.User));

        if (!roleAddResult.Succeeded)
        {
            var identityError = roleAddResult.Errors.FirstOrDefault();
            return Result.Failure(identityError != null ? new Error(identityError.Code, identityError.Description) : Error.Unknown);
        }

        return Result.Success();
    }

    #endregion
}
