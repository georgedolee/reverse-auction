using MediatR;
using Microsoft.AspNetCore.Http;

namespace UserService.Application.Features.Users.Commands.UploadPhoto;

public sealed class UploadUserPhotoCommand : IRequest<bool>
{
    public UploadUserPhotoCommand(Guid id, IFormFile file)
    {
        Id = id;
        File = file;
    }

    public Guid Id { get; set; }

    public IFormFile File { get; set; }
}
