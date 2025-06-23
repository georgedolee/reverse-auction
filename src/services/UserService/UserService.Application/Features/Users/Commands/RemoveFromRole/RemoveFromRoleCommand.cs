using MediatR;

namespace UserService.Application.Features.Users.Commands.RemoveFromRole;

public sealed class RemoveFromRoleCommand : IRequest<bool>
{
    public Guid UserId { get; set; }

    public string RoleName { get; set; } = string.Empty;
}
