using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Authorization;

public class GrpcResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly AuthService.AuthServiceClient _client;
    private readonly ILogger<GrpcResourceOwnerPasswordValidator> _logger;

    public GrpcResourceOwnerPasswordValidator(AuthService.AuthServiceClient client, ILogger<GrpcResourceOwnerPasswordValidator> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        _logger.LogInformation("Starting credential validation for {Username}", context.UserName);

        var grpcRequest = new ValidateCredentialsRequest
        {
            Username = context.UserName,
            Password = context.Password
        };

        var grpcResponse = await _client.ValidateCredentialsAsync(grpcRequest);

        if (grpcResponse.Success)
        {
            _logger.LogInformation("Credential validation successful for {Username}", context.UserName);

            context.Result = new GrantValidationResult(
                subject: grpcResponse.UserId,
                authenticationMethod: "custom_grpc"
            );
        }
        else
        {
            _logger.LogWarning("Credential validation failed for {Username}", context.UserName);
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
        }
    }
}
