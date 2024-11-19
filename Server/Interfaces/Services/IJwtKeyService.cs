using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Server.Interfaces.Services;

public interface IJwtKeyService
{
    public TokenValidationParameters GetTokenValidationParameters();
    public string GetAccessToken(List<Claim> claims);
    public string GetRefreshToken(List<Claim> claims);
}
