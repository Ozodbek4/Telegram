using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Telegram.Application.Common.Services;
using Telegram.Application.Common.Settings;
using Telegram.Domain.Entities;

namespace Telegram.Infrastructure.Common.Services;

public class TokenGeneratorService(IOptions<JwtSettings> jwtSettings) : ITokenGeneratorService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public ValueTask<string> GenerateToken(User user, CancellationToken cancellationToken = default)
    {
        var jwtToken = GetJwtToken(user);

        return new (new JwtSecurityTokenHandler().WriteToken(jwtToken));
    }

    private JwtSecurityToken GetJwtToken(User user)
    {
        var security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credential = new SigningCredentials(security, SecurityAlgorithms.HmacSha256);
        var claims = GetClaims(user);

        return new JwtSecurityToken(
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationTimeInMinutes),
            signingCredentials: credential
            );
    }

    private List<Claim> GetClaims(User user) => new List<Claim>
    {
        new Claim("UserId", user.Id.ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.EmailAddress),
    };
}
