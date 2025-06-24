using MediatR;
using UserService.Application.Contracts;

namespace UserService.Application.Features.Users.Commands.RemoveFromRole;

public sealed class RemoveFromRoleCommand : RoleDto, IRequest<bool>
{
    public RemoveFromRoleCommand(Guid userId, string roleName)
    {
        UserId = userId;
        RoleName = roleName;
    }

    public Guid UserId { get; set; }
}
