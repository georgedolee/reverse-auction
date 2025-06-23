using FluentValidation;
using SharedInfrastructure.Extensions;

namespace UserService.Application.Features.Users.Commands.Delete;

public sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(x => x.Id).ValidId();
    }
}
