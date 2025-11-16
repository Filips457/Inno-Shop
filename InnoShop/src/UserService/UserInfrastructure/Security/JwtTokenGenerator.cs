using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserApplication.DTOs;
using UserApplication.Interfaces;

namespace UserInfrastructure.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings settings;

    public JwtTokenGenerator(IOptions<JwtSettings> options)
    {
        settings = options.Value;
    }

    public string GenerateToken(UserDTO user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.UserRole.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                            issuer: settings.Issuer,
                            audience: settings.Audience,
                            claims: claims,
                            notBefore: DateTime.UtcNow,
                            expires: DateTime.UtcNow.AddMinutes(settings.ExpiryMinutes),
                            signingCredentials: creds);


        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}