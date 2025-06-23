using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.Application.Contracts.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Roles.Commands.Create;

internal sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleModel>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<CreateRoleCommandHandler> _logger;

    public CreateRoleCommandHandler(
        RoleManager<Role> roleManager,
        ILogger<CreateRoleCommandHandler> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<RoleModel> Handle(CreateRoleCommand command, CancellationToken ct)
    {
        _logger.LogInformation("Creating new role with name {NewRoleName}.", command.Name);
        var newRole = new Role
        {
            Name = command.Name
        };

        var result = await _roleManager.CreateAsync(newRole);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Role with name {RoleName} couldnot be created.", command.Name);
            throw new InvalidOperationException(result.Errors.First().Description);
        }

        _logger.LogInformation("New role with name {NewRoleName} created successfully.", newRole.Name);
        return newRole.Adapt<RoleModel>();
    }
}
