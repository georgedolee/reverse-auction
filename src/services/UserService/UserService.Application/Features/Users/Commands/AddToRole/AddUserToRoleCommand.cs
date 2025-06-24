using MediatR;
using UserService.Application.Contracts;

namespace UserService.Application.Features.Users.Commands.AddToRole;

public sealed class AddUserToRoleCommand : RoleDto, IRequest<bool>
{
    public AddUserToRoleCommand(Guid userId, string roleName)
    {
        UserId = userId;
        RoleName = roleName;
    }

    public Guid UserId { get; set; }
}
