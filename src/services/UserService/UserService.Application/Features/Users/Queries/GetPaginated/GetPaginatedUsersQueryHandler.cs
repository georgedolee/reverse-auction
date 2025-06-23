using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel.Results;
using UserService.Application.Contracts.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users.Queries.GetPaginated;

internal sealed class GetPaginatedUsersQueryHandler : IRequestHandler<GetPaginatedUsersQuery, PagedResult<UserModel>>
{
    private readonly ILogger<GetPaginatedUsersQueryHandler> _logger;
    private readonly UserManager<User> _userManager;

    public GetPaginatedUsersQueryHandler(
        ILogger<GetPaginatedUsersQueryHandler> logger,
        UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<PagedResult<UserModel>> Handle(GetPaginatedUsersQuery query, CancellationToken ct)
    {
        _logger.LogInformation(
            "Fetching users on page {PageNumber} with page size {PageSize}", 
            query.PageNumber, query.PageSize);

        var totalCount = await _userManager.Users.CountAsync(ct);
        IEnumerable<User> pagedUsers = [];

        if (totalCount != 0)
        {
            pagedUsers = await _userManager.Users
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(ct);
        }

        _logger.LogInformation(
            "Users on page {PageNumber} with page size {PageSize} fetched successfully.",
            query.PageNumber, query.PageSize);

        return new PagedResult<UserModel>(
            items: pagedUsers.Adapt<List<UserModel>>(),
            totalCount: totalCount,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize);
    }
}
