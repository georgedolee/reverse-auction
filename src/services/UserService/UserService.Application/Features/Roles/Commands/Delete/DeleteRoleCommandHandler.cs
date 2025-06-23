using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Guards;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Roles.Commands.Delete;

internal sealed class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, bool>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<DeleteRoleCommandHandler> _logger;

    public DeleteRoleCommandHandler(
        RoleManager<Role> roleManager,
        ILogger<DeleteRoleCommandHandler> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteRoleCommand command, CancellationToken ct)
    {
        _logger.LogInformation("Fetching role with id {RoleId}.", command.Id);
        var role = await _roleManager.FindByIdAsync(command.Id.ToString());

        Guard.EnsureFound(role, nameof(role), command.Id, _logger);

        var result = await _roleManager.DeleteAsync(role!);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Role with id {RoleId} couldnot be deleted.", command.Id);
            throw new InvalidOperationException(result.Errors.First().Description);
        }

        _logger.LogInformation("Role with id {RoleId} deleted successfully.", command.Id);
        return true;
    }
}
