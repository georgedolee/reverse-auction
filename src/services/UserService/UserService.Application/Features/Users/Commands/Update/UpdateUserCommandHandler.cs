using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Guards;
using UserService.Application.Contracts.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users.Commands.Update;

internal sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserModel>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(
        UserManager<User> userManager,
        ILogger<UpdateUserCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<UserModel> Handle(UpdateUserCommand command, CancellationToken ct)
    {
        _logger.LogInformation("Fetching user with id {UserId}.", command.Id);
        var user = await _userManager.FindByIdAsync(command.Id.ToString());

        Guard.EnsureFound(user, nameof(user), command.Id, _logger);

        _logger.LogInformation("Updating user with properties {@Properties}.", command);
        user!.UserName = command.UserName;
        user.Email = command.Email;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogWarning(
               "User with username {UserName} and email {Email} couldnot be Updated.",
               user.UserName, user.Email);

            throw new InvalidOperationException(result.Errors.First().Description);
        }

        _logger.LogInformation("User with id {UserId} updated successfully.", command.Id);
        return user.Adapt<UserModel>();
    }
}
