using MediatR;

namespace UserService.Application.Features.Users.Commands.Delete;

public sealed class DeleteUserCommand : IRequest<bool>
{
    public DeleteUserCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}