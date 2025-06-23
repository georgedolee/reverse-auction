using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Guards;
using UserService.Application.Contracts.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Roles.Queries.GetById;

internal sealed class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleModel>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<GetRoleByIdQueryHandler> _logger;

    public GetRoleByIdQueryHandler(
        RoleManager<Role> roleManager,
        ILogger<GetRoleByIdQueryHandler> logger)
    {
        _logger = logger;
        _roleManager = roleManager;
    }

    public async Task<RoleModel> Handle(GetRoleByIdQuery query, CancellationToken ct)
    {
        _logger.LogInformation("Fetching role with id {RoleId}.", query.Id);
        var role = await _roleManager.FindByIdAsync(query.Id.ToString());

        Guard.EnsureFound(role, nameof(role), query.Id, _logger);

        _logger.LogInformation("Role with id {RoleId} fetched successfully.", query.Id);
        return role.Adapt<RoleModel>();
    }
}
