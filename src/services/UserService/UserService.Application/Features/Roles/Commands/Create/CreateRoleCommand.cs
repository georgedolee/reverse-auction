using MediatR;
using UserService.Application.Contracts.Models;

namespace UserService.Application.Features.Roles.Commands.Create;

public sealed class CreateRoleCommand : IRequest<RoleModel>
{
    public string Name { get; set; } = string.Empty;
}
