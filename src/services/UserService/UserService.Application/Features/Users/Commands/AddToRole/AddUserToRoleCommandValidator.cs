using FluentValidation;
using SharedInfrastructure.Extensions;

namespace UserService.Application.Features.Users.Commands.AddToRole;

public sealed class AddUserToRoleCommandValidator : AbstractValidator<AddUserToRoleCommand>
{
    public AddUserToRoleCommandValidator()
    {
        RuleFor(x => x.UserId).ValidId("UserId");

        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("RoleName can't be empty.");
    }
}
