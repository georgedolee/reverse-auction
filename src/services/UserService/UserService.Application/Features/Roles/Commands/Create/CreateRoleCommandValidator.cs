using FluentValidation;

namespace UserService.Application.Features.Roles.Commands.Create;

public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name can't be empty.");
    }
}
