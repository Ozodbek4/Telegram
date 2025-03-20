using Telegram.Application.Common.Identity;
using Telegram.Application.Settings;
using Telegram.Domain.Entities;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Telegram.Infrastructure.Common.Identity;

public class TokenGeneratorService : ITokenGeneratorService
{
    private readonly JwtSettings _jwtSettings;

    public TokenGeneratorService(IOptions<JwtSettings> jwtSettings) =>
        _jwtSettings = jwtSettings.Value;

    public Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken = default)
    {
        var jwtSecurityToken = JwtSecurityToken(user);
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return Task.FromResult(token);
    }

    private JwtSecurityToken JwtSecurityToken(User user)
    {
        var security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(security, SecurityAlgorithms.HmacSha256);
        var claims = GetClaims(user);

        return new JwtSecurityToken(
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddSeconds(_jwtSettings.ExpirationTimeInSeconds),
            signingCredentials: credentials
            );
    }

    private IList<Claim> GetClaims(User user)
    {
        return new List<Claim>
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
        };
    }
}