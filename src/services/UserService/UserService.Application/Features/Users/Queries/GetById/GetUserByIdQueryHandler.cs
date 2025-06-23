using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SharedKernel.Guards;
using UserService.Application.Contracts.Models;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users.Queries.GetById;

internal sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserModel>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(
        UserManager<User> userManager,
        ILogger<GetUserByIdQueryHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<UserModel> Handle(GetUserByIdQuery query, CancellationToken ct)
    {
        _logger.LogInformation("Fetching user with id {UserId}.", query.Id);
        var user = await _userManager.FindByIdAsync(query.Id.ToString());

        Guard.EnsureFound(user, nameof(user), query.Id, _logger);

        var roles = await _userManager.GetRolesAsync(user!);

        var userModel = user.Adapt<UserModel>();
        userModel.Roles = roles.Adapt<IEnumerable<string>>();

        _logger.LogInformation("User with id {UserId} fetched successfully.", query.Id);
        return userModel;
    }
}
