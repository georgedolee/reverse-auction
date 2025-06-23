using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel.Results;
using UserService.Application.Contracts.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Roles.Queries.GetPaginated;

internal sealed class GetPaginatedRolesQueryHandler : IRequestHandler<GetPaginatedRolesQuery, PagedResult<RoleModel>>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<GetPaginatedRolesQueryHandler> _logger;

    public GetPaginatedRolesQueryHandler(
        RoleManager<Role> roleManager,
        ILogger<GetPaginatedRolesQueryHandler> logger)
    {
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<PagedResult<RoleModel>> Handle(GetPaginatedRolesQuery query, CancellationToken ct)
    {
        _logger.LogInformation(
            "Fetching roles on page {PageNumber} with page size {PageSize}",
            query.PageNumber, query.PageSize);

        var totalCount = await _roleManager.Roles.CountAsync(ct);
        IEnumerable<IdentityRole<Guid>> pagedRoles = [];

        if (totalCount != 0)
        {
            pagedRoles = await _roleManager.Roles
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(ct);
        }

        _logger.LogInformation(
            "Roles on page {PageNumber} with page size {PageSize} fetched successfully.",
            query.PageNumber, query.PageSize);

        return new PagedResult<RoleModel>(
            items: pagedRoles.Adapt<List<RoleModel>>(),
            totalCount: totalCount,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize);
    }
}
