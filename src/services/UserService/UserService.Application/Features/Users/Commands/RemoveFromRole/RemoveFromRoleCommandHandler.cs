using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Guards;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users.Commands.RemoveFromRole;

internal sealed class RemoveFromRoleCommandHandler : IRequestHandler<RemoveFromRoleCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RemoveFromRoleCommandHandler> _logger;

    public RemoveFromRoleCommandHandler(
        UserManager<User> userManager,
        ILogger<RemoveFromRoleCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    public async Task<bool> Handle(RemoveFromRoleCommand command, CancellationToken ct)
    {
        _logger.LogInformation("Fetching user with id {UserId}.", command.UserId);
        var user = await _userManager.FindByIdAsync(command.UserId.ToString());

        Guard.EnsureFound(user, nameof(user), command.UserId, _logger);

        _logger.LogInformation(
            "Removing user with id {UserId} from the role {RoleName}.",
            command.UserId, command.RoleName);

        var result = await _userManager.RemoveFromRoleAsync(user!, command.RoleName);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(result.Errors.First().Description);
        }

        _logger.LogInformation(
            "User with id {UserId} successfully removed from the role {RoleName}.",
            command.UserId, command.RoleName);

        return true;
    }
}
