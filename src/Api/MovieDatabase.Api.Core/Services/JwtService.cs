using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using MovieDatabase.Api.Core.Documents.Users;
using MovieDatabase.Api.Core.Jwt;

namespace MovieDatabase.Api.Core.Services;

public class JwtService(IOptions<JwtSettings> options) : IJwtService
{
    private readonly JwtSettings _settings = options.Value;

    public (string? token, DateTime expireDate) GenerateJwtToken(User user)
    {
        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(_settings.ExpirationMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Name),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtExtendedClaimTypes.Kid, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, user.Role.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return (tokenString, expires);
    }
}