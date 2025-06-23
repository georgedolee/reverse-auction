using MediatR;

namespace UserService.Application.Features.Roles.Commands.Delete;

public sealed class DeleteRoleCommand : IRequest<bool>
{
    public DeleteRoleCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
