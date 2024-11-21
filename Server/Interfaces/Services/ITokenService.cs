using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Server.Interfaces.Services;

public interface ITokenService
{
    public TokenValidationParameters GetTokenValidationParameters();
    public string GetAccessToken(IEnumerable<Claim> claims);
    public string GetRefreshToken(IEnumerable<Claim> claims);
    public Task<TokenValidationResult> ValidateToken(string token);
}
