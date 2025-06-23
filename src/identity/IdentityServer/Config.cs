using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = "role",
                UserClaims = new List<string> {"role"}
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("testapi:read"),
            new ApiScope("testapi:write"),
        };

    public static IEnumerable<ApiResource> ApiResources => new[]
    {
        new ApiResource("testapi")
        {
            Scopes = new List<string> {"testapi:read", "testapi:write"},
            ApiSecrets = new List<Secret> {new Secret("scopesecret".Sha256())},
            UserClaims = new List<string> {"role"}
        }
    };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "ios_app",
                ClientSecrets = { new Secret("af2a447d-5ccd-4b35-a1d4-31bfdc41aade".Sha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
    
                AllowedScopes =
                {
                    "openid",
                    "profile",
                    "offline_access",
                    "testapi.read",
                    "testapi.write"
                },

                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                AccessTokenLifetime = 3600,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime = 5600,
            }
        };
}
