using IdentityServer4.Models;

public static class Config
{
    public static List<Client> Clients = new List<Client>
    {
            new Client
            {
                ClientId = "identity-server-demo-web",
                AllowedGrantTypes = new List<string> { GrantType.AuthorizationCode },
                RequireClientSecret = false,
                RequireConsent = false,
                RedirectUris = new List<string> { "http://localhost:3006/signin-callback.html" },
                PostLogoutRedirectUris = new List<string> { "http://localhost:3006/" },
                AllowedScopes = { "identity-server-demo-api","resource-three", "resource-two" ,"write", "read", "openid", "profile", "email" },
                AllowedCorsOrigins = new List<string>
                {
                    "http://localhost:3006",
                },
                RequirePkce = false,
                AccessTokenLifetime = 86400
            },
                       new Client
            {
                ClientId = "implicit-web",
                AllowedGrantTypes = new List<string> { GrantType.Implicit,GrantType.ResourceOwnerPassword },
                RequireClientSecret = false,
                RequireConsent = false,
                RedirectUris = new List<string> { "http://localhost:4200" },
                PostLogoutRedirectUris = new List<string> { "http://localhost:4200/" },
                AllowedScopes = { "resource-three", "write", "read", "openid", "profile", "email" },
                AllowedCorsOrigins = new List<string>
                {
                    "http://localhost:4200",
                },
                AccessTokenLifetime = 86400
            },
                                  new Client
            {
                ClientId = "backtoback",
                AllowedGrantTypes = new List<string> { GrantType.ClientCredentials },
                RequireClientSecret = false,
                RequireConsent = false,
                AllowedScopes = { "resource-two", "write", "read", "profile", "email" },
                AccessTokenLifetime = 86400
            }
    };

    public static List<ApiResource> ApiResources = new List<ApiResource>
    {
        new ApiResource
        {
            Name = "identity-server-demo-api",
            DisplayName = "Identity Server Demo API",
            Scopes = new List<string>
            {
                "write",
                "read"
            }
        },
                new ApiResource
        {
            Name = "resource-two",
            DisplayName = "Resource 2 API",
            Scopes = new List<string>
            {
                "write",
                "read"
            }
        },
                              new ApiResource
        {
            Name = "resource-three",
            DisplayName = "Resource 3 API",
            Scopes = new List<string>
            {
                "write",
                "read"
            }
        }
    };

    public static IEnumerable<ApiScope> ApiScopes = new List<ApiScope>
    {
        new ApiScope("openid"),
        new ApiScope("profile"),
        new ApiScope("email"),
        new ApiScope("read"),
        new ApiScope("write"),
        new ApiScope("identity-server-demo-api"),
        new ApiScope("resource-two",new string[]{"Address"}),
        new ApiScope("resource-three",new string[]{"SSN"}),
    };
}