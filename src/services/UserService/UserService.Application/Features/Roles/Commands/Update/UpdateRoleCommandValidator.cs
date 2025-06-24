using FluentValidation;
using SharedInfrastructure.Extensions;

namespace UserService.Application.Features.Roles.Commands.Update;

public sealed class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Id).ValidId();

        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("RoleName can't be empty.");
    }
}
