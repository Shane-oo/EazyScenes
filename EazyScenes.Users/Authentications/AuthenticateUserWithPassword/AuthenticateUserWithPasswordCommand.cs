using EazyScenes.Core.Exchange;
using FluentValidation;
using JetBrains.Annotations;

namespace EazyScenes.Users.Authentications.AuthenticateUserWithPassword;

public class AuthenticateUserWithPasswordCommand: Command<UserAuthenticationPayload>
{
    #region Properties

    public string Password { get; }

    public string UserName { get; }

    #endregion

    #region Construction

    public AuthenticateUserWithPasswordCommand(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    #endregion
}

[UsedImplicitly]
public class AuthenticateUserWithPasswordCommandValidator: AbstractValidator<AuthenticateUserWithPasswordCommand>
{
    public AuthenticateUserWithPasswordCommandValidator()
    {
        RuleFor(c => c.UserName).NotEmpty();
        RuleFor(c => c.Password).NotEmpty();
    }
}
