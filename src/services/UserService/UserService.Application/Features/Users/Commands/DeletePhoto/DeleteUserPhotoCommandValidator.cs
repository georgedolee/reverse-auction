using FluentValidation;
using SharedInfrastructure.Extensions;

namespace UserService.Application.Features.Users.Commands.DeletePhoto;

public sealed class DeleteUserPhotoCommandValidator : AbstractValidator<DeleteUserPhotoCommand>
{
    public DeleteUserPhotoCommandValidator()
    {
        RuleFor(x => x.Id).ValidId();
    }
}
