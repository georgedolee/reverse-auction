using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Guards;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users.Commands.AddToRole;

internal sealed class AddUserToRoleCommandHandler : IRequestHandler<AddUserToRoleCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AddUserToRoleCommandHandler> _logger;

    public AddUserToRoleCommandHandler(
        UserManager<User> userManager,
        ILogger<AddUserToRoleCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    public async Task<bool> Handle(AddUserToRoleCommand command, CancellationToken ct)
    {
        _logger.LogInformation("Fetching user with id {UserId}.", command.UserId);
        var user = await _userManager.FindByIdAsync(command.UserId.ToString());

        Guard.EnsureFound(user, nameof(user), command.UserId, _logger);

        _logger.LogInformation(
            "Adding user with id {UserId} to the role {RoleName}.", 
            command.UserId, command.RoleName);

        var result = await _userManager.AddToRoleAsync(user!, command.RoleName);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(result.Errors.First().Description);
        }

        _logger.LogInformation(
            "User with id {UserId} successfully added to the role {RoleName}.", 
            command.UserId, command.RoleName);

        return true;
    }
}
