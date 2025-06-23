using FluentValidation;
using SharedInfrastructure.Extensions;

namespace UserService.Application.Features.Users.Commands.RemoveFromRole;

public sealed class RemoveFromRoleCommandValidator : AbstractValidator<RemoveFromRoleCommand>
{
    public RemoveFromRoleCommandValidator()
    {
        RuleFor(x => x.UserId).ValidId("UserId");

        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("RoleName can't be empty.");
    }
}
