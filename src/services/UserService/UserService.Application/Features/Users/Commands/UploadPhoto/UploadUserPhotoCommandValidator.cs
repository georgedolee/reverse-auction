using FluentValidation;
using SharedInfrastructure.Extensions;

namespace UserService.Application.Features.Users.Commands.UploadPhoto;

public sealed class UploadUserPhotoCommandValidator : AbstractValidator<UploadUserPhotoCommand>
{
    public UploadUserPhotoCommandValidator()
    {
        RuleFor(x => x.Id).ValidId();

        RuleFor(x => x.File)
            .NotEmpty().WithMessage("file can't be empty.");
    }
}
