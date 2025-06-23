using FluentValidation;
using SharedInfrastructure.Extensions;

namespace UserService.Application.Features.Users.Commands.Update;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id).ValidId();

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName cant't be empty")
            .MaximumLength(50).WithMessage("UserName can't be more than 50 characters.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email should be correct email address format");
    }
}
