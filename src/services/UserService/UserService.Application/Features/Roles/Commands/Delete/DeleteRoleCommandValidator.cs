using FluentValidation;
using SharedInfrastructure.Extensions;

namespace UserService.Application.Features.Roles.Commands.Delete;

public sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(x => x.Id).ValidId();
    }
}
