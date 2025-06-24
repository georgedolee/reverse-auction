using MediatR;
using UserService.Application.Contracts;
using UserService.Application.Contracts.Models;

namespace UserService.Application.Features.Roles.Commands.Update;

public sealed class UpdateRoleCommand : RoleDto, IRequest<RoleModel>
{
    public UpdateRoleCommand(Guid id, string roleName)
    {
        Id = id;
        RoleName = roleName;
    }

    public Guid Id { get; set; }
}
