using MediatR;
using UserService.Application.Contracts.Models;
using UserService.Application.Contracts.Requests;

namespace UserService.Application.Features.Roles.Commands.Update;

public sealed class UpdateRoleCommand : UpdateRoleRequest, IRequest<RoleModel>
{
    public UpdateRoleCommand(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; set; }
}
