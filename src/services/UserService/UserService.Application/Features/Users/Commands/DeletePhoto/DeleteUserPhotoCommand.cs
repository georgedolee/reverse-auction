using MediatR;

namespace UserService.Application.Features.Users.Commands.DeletePhoto;

public sealed class DeleteUserPhotoCommand : IRequest<bool>
{
    public DeleteUserPhotoCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
