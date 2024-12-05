using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Interfaces.Services;
using Server.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Services;

public class TokenService : ITokenService
{
    public TokenService(AuthSettings authSettings)
    {
        _authSettings = authSettings;
    }

    public TokenService(IOptions<AuthSettings> options)
    {
        _authSettings = options.Value;
    }

    public TokenValidationParameters GetTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _authSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = _authSettings.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    }

    public string GetAccessToken(IEnumerable<Claim> claims)
    {
        var expiresTime = TimeSpan.FromMinutes(_authSettings.AccessTokenExpiresTimeInMinutes);
        return CreateToken(claims, expiresTime);
    }

    public string GetRefreshToken(IEnumerable<Claim> claims)
    {
        var expiresTime = TimeSpan.FromMinutes(_authSettings.RefreshTokenExpiresTimeInMinutes);
        return CreateToken(claims, expiresTime);
    }

    string CreateToken(IEnumerable<Claim> claims, TimeSpan expiresTime)
    {
        var signingCredentials = new SigningCredentials(
            GetSymmetricSecurityKey(),
            SecurityAlgorithms.HmacSha256
        );

        var securityToken = new JwtSecurityToken(
            issuer: _authSettings.Issuer,
            audience: _authSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(expiresTime),
            signingCredentials: signingCredentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(securityToken);
    }

    SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(_authSettings.Key);
        return new SymmetricSecurityKey(key);
    }

    public async Task<TokenValidationResult> ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        return await tokenHandler.ValidateTokenAsync(
            token, GetTokenValidationParameters()
        );
    }

    readonly AuthSettings _authSettings;
}
