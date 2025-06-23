using Grpc.Core;
using Authorization;
using Microsoft.AspNetCore.Identity;
using UserService.Domain.Entities;
using User = UserService.Domain.Entities.User;

namespace UserService.API.GrpcServices;

public class GrpcAuthService : AuthService.AuthServiceBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<GrpcAuthService> _logger;

    public GrpcAuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<Role> roleManager,
        ILogger<GrpcAuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public override async Task<ValidateCredentialsResponse> ValidateCredentials(ValidateCredentialsRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Validating credentials for user: {Username}", request.Username);

        var user = await _userManager.FindByNameAsync(request.Username);

        if (user == null)
        {
            _logger.LogWarning("User {Username} not found", request.Username);
            return new ValidateCredentialsResponse { Success = false };
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!signInResult.Succeeded)
        {
            _logger.LogWarning("Invalid password for user: {Username}", request.Username);
            return new ValidateCredentialsResponse { Success = false };
        }

        _logger.LogInformation("Credentials valid for user: {Username}", request.Username);

        return new ValidateCredentialsResponse
        {
            Success = true,
            UserId = user.Id.ToString()
        };
    }

    public override async Task<GetUserInfoResponse> GetUserInfo(GetUserInfoRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Fetching user info for user ID: {UserId}", request.UserId);

        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user == null)
        {
            _logger.LogWarning("User ID {UserId} not found", request.UserId);
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        }

        var roles = await _userManager.GetRolesAsync(user);
        var userClaims = await _userManager.GetClaimsAsync(user);

        var roleClaims = new List<System.Security.Claims.Claim>();
        foreach (var roleName in roles)
        {
            var roleEntity = await _roleManager.FindByNameAsync(roleName);
            if (roleEntity != null)
            {
                roleClaims.AddRange(await _roleManager.GetClaimsAsync(roleEntity));
            }
        }

        var allClaims = userClaims.Concat(roleClaims).Select(c => new Claim
        {
            Type = c.Type,
            Value = c.Value
        });

        var grpcUser = new Authorization.User
        {
            Id = user.Id.ToString(),
            Username = user.UserName
        };

        grpcUser.Roles.AddRange(roles);
        grpcUser.Claims.AddRange(allClaims);

        return new GetUserInfoResponse { User = grpcUser };
    }
}
