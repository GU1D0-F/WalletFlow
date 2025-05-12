using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WalletFlow.Domain.Entities.Users;

namespace WalletFlow.Application.Services.Identity.JwtToken;

public class JwtTokenService(IConfiguration config) : IJwtTokenService
{
    private readonly SymmetricSecurityKey _key = new(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
    private readonly TimeSpan _tokenLifetime = TimeSpan.FromHours(2);
    private readonly string _issuer = config["Jwt:Issuer"]!;

    public (string Token, DateTime ExpiresAt) GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.UserName!)
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.Add(_tokenLifetime);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject            = new ClaimsIdentity(claims),
            Expires            = expires,
            SigningCredentials = credentials,
            Issuer             = _issuer,
            Audience           = _issuer
        };

        var handler = new JwtSecurityTokenHandler();
        var token   = handler.CreateToken(tokenDescriptor);
        return (handler.WriteToken(token), expires);
    }
}