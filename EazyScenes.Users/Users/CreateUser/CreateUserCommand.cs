using EazyScenes.Core.Exchange;
using FluentValidation;
using JetBrains.Annotations;

namespace EazyScenes.Users.Users.CreateUser;

public class CreateUserCommand: Command
{
    #region Properties

    public string ConfirmPassword { get; }

    public string Email { get; }

    public string Password { get; }

    public string UserName { get; }

    #endregion

    #region Construction

    public CreateUserCommand(string email, string userName, string password, string confirmPassword)
    {
        Email = email;
        UserName = userName;
        Password = password;
        ConfirmPassword = confirmPassword;
    }

    #endregion
}

[UsedImplicitly]
public class CreateUserCommandValidator: AbstractValidator<CreateUserCommand>
{
    #region Construction

    public CreateUserCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(c => c.UserName)
            .NotEmpty()
            .MaximumLength(31);
        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(8);
        RuleFor(c => c.ConfirmPassword)
            .NotEmpty()
            .Matches(d => d.Password).WithMessage("Confirmation Password Must Match Password");
    }

    #endregion
}
