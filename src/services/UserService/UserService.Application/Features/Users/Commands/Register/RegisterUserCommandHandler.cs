using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users.Commands.Register;

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(
        UserManager<User> userManager,
        ILogger<RegisterUserCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<bool> Handle(RegisterUserCommand command, CancellationToken ct)
    {
        _logger.LogInformation(
            "Registering new user with username {UserName} and email {Email}.", 
            command.UserName, command.Email);

        var newUser = new User
        {
            UserName = command.UserName,
            Email = command.Email
        };

        var result = await _userManager.CreateAsync(newUser, command.Password);

        if (!result.Succeeded)
        {
            _logger.LogWarning(
                "User with username {UserName} and email {Email} couldnot be registered.",
                command.UserName, command.Email);

            throw new InvalidOperationException(result.Errors.First().Description);
        }

        _logger.LogInformation(
            "User with username {UserName} and email {Email} registered successfully.",
            command.UserName, command.Email);

        return result.Succeeded;
    }
}
