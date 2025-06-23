using MediatR;

namespace UserService.Application.Features.Users.Commands.AddToRole;

public sealed class AddUserToRoleCommand : IRequest<bool>
{
    public Guid UserId { get; set; }

    public string RoleName { get; set; } = string.Empty;
}
