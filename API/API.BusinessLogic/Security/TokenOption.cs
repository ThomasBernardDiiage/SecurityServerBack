using System;
namespace API.BusinessLogic.Security;

public class TokenOptions
{
    public string Audience { get; set; } = "IdentityServer";
    public string Issuer { get; set; } = "IdentityServer";
    public long AccessTokenExpiration { get; set; } = 86400;
    public long RefreshTokenExpiration { get; set; } = 172800;
    public string Secret { get; set; } = "very_long_but_insecure_token_here_be_sure_to_use_env_var";
}
