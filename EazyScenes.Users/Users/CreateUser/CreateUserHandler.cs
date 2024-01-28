using EazyScenes.Core;
using EazyScenes.Core.Exchange;
using EazyScenes.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace EazyScenes.Users.Users.CreateUser;

public class CreateUserHandler: ICommandHandler<CreateUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public CreateUserHandler(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var user = new User
                   {
                       Id = new UserId(Guid.NewGuid()),
                       Email = command.Email,
                       UserName = command.UserName,
                       CreatedOn = DateTimeOffset.UtcNow // use entity framework core interceptor
                   };
        var userCreateResult = await _userManager.CreateAsync(user, command.Password);

        if (!userCreateResult.Succeeded)
        {
            var identityError = userCreateResult.Errors.FirstOrDefault();
            return Result.Failure(identityError != null ? new Error(identityError.Code, identityError.Description) : Error.Unknown);
        }

        // Do the Same For Admin todo
        const string ROLE_NAME = nameof(Roles.Admin);
        var roleExists = await _roleManager.FindByNameAsync(ROLE_NAME) != null;
        if (!roleExists)
        {
            var role = new Role
                       {
                           Name = ROLE_NAME
                       };
            await _roleManager.CreateAsync(role);
        }

        var roleAddResult = await _userManager.AddToRoleAsync(user, ROLE_NAME);

        if (!roleAddResult.Succeeded)
        {
            var identityError = roleAddResult.Errors.FirstOrDefault();
            return Result.Failure(identityError != null ? new Error(identityError.Code, identityError.Description) : Error.Unknown);
        }

        return Result.Success();
    }
}
