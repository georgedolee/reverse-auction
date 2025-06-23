using FluentValidation;
using SharedInfrastructure.Extensions;

namespace UserService.Application.Features.Roles.Commands.Update;

public sealed class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Id).ValidId();

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name can't be empty.");
    }
}
