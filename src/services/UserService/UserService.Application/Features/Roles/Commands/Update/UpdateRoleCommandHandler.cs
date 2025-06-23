using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Guards;
using UserService.Application.Contracts.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Roles.Commands.Update;

internal sealed class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleModel>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<UpdateRoleCommandHandler> _logger;

    public UpdateRoleCommandHandler(
        RoleManager<Role> roleManager,
        ILogger<UpdateRoleCommandHandler> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<RoleModel> Handle(UpdateRoleCommand command, CancellationToken ct)
    {
        _logger.LogInformation("Fetching role with id {RoleId}.", command.Id);
        var role = await _roleManager.FindByIdAsync(command.Id.ToString());

        Guard.EnsureFound(role, nameof(role), command.Id, _logger);

        _logger.LogInformation("Updating role with properties {@Properties}.", command);
        role!.Name = command.Name;

        var result = await _roleManager.UpdateAsync(role);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Role with id {RoleId} couldnot be updated.", command.Id);
            throw new InvalidOperationException(result.Errors.First().Description);
        }

        _logger.LogInformation("Role with id {RoleId} updated successfully.", command.Id);
        return role.Adapt<RoleModel>();
    }
}