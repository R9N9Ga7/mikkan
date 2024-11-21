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
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _authSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = _authSettings.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
        return parameters;
    }

    public string GetAccessToken(IEnumerable<Claim> claims)
    {
        var expiresTime = TimeSpan.FromMinutes(_authSettings.AccessTokenExpiresTimeInMinutes);
        var token = CreateToken(claims, expiresTime);
        return token;
    }

    public string GetRefreshToken(IEnumerable<Claim> claims)
    {
        var expiresTime = TimeSpan.FromMinutes(_authSettings.RefreshTokenExpiresTimeInMinutes);
        var token = CreateToken(claims, expiresTime);
        return token;
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
        var token = tokenHandler.WriteToken(securityToken);
        return token;
    }

    SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(_authSettings.Key);
        var symmetricSecurityKey = new SymmetricSecurityKey(key);
        return symmetricSecurityKey;
    }

    public async Task<TokenValidationResult> ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationResult = await tokenHandler.ValidateTokenAsync(
            token, GetTokenValidationParameters());
        return tokenValidationResult;
    }

    readonly AuthSettings _authSettings;
}
