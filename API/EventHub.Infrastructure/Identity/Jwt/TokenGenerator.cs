using System.Security.Claims;
using System.Text;
using EventHub.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
namespace EventHub.Infrastructure.Identity.Jwt;

public class TokenGenerator: ITokenGenerator
{
    private readonly IConfiguration _config;

    public TokenGenerator(IConfiguration config)
    {
        _config=config;
    }

    public string GenerateToken(string userId, string email, IList<string> roles)
    {
        var claims= new List<Claim>
        {
            new (ClaimTypes.NameIdentifier,userId),
            new (ClaimTypes.Email, email)
        };
        foreach(var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!));

        var credentials= new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token= new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims:claims,
            expires:DateTime.UtcNow.AddMinutes(15),
            signingCredentials:credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public string RefreshToken()
    {
          return Convert.ToBase64String(
        RandomNumberGenerator.GetBytes(64));
    }
}