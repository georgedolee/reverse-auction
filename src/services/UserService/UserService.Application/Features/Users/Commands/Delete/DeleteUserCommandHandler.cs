using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Guards;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users.Commands.Delete;

internal sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(
        ILogger<DeleteUserCommandHandler> logger,
        UserManager<User> userManager)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteUserCommand command, CancellationToken ct)
    {
        _logger.LogInformation("Fetching user with id {UserId}.", command.Id);
        var user = await _userManager.FindByIdAsync(command.Id.ToString());

        Guard.EnsureFound(user, nameof(user), command.Id, _logger);

        _logger.LogInformation("Deleting user with id {UserId}.", user!.Id);
        var result = await _userManager.DeleteAsync(user!);

        if (!result.Succeeded)
        {
            _logger.LogWarning(
                "User with id {UserId}.",
                user.Id);

            throw new InvalidOperationException(result.Errors.First().Description);
        }

        _logger.LogInformation("User with id {UserId} deleted successfully.", user!.Id);
        return true;
    }
}
