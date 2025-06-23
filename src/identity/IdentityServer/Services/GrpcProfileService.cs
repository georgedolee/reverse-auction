using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Authorization;
using Duende.IdentityModel;
using Claim = System.Security.Claims.Claim;

namespace IdentityServer.Services;

public class GrpcProfileService : IProfileService
{
    private readonly AuthService.AuthServiceClient _client;
    private readonly ILogger<GrpcProfileService> _logger;

    public GrpcProfileService(AuthService.AuthServiceClient client, ILogger<GrpcProfileService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var userId = context.Subject.FindFirst(JwtClaimTypes.Subject)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("No subject claim found in context");
            return;
        }

        var grpcRequest = new GetUserInfoRequest { UserId = userId };
        var grpcResponse = await _client.GetUserInfoAsync(grpcRequest);

        var claims = new List<Claim>
        {
            new Claim(JwtClaimTypes.Subject, grpcResponse.User.Id),
            new Claim(JwtClaimTypes.PreferredUserName, grpcResponse.User.Username)
        };

        claims.AddRange(grpcResponse.User.Roles.Select(role => new Claim(JwtClaimTypes.Role, role)));
        claims.AddRange(grpcResponse.User.Claims.Select(c => new Claim(c.Type, c.Value)));

        context.IssuedClaims = claims;
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;
        return Task.CompletedTask;
    }
}
