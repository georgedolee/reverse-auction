using FluentValidation;

namespace UserService.Application.Features.Users.Commands.Register;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName cant't be empty")
            .MaximumLength(50).WithMessage("UserName can't be more than 50 characters.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email should be correct email address format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password can't be empty")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.");
    }
}
