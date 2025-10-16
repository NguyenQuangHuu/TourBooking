using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using QuanLySanPham.Application.Services;
using QuanLySanPham.Domain.Aggregates.Auth;
using QuanLySanPham.Domain.ValueObjects;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace QuanLySanPham.Infrastructure.Security;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken(User user)
    {
        var key = _configuration["Jwt:Key"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expiresMinutes = int.Parse(_configuration["Jwt:ExpiresMinutes"] ?? "20");

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var cre = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>();
        if (user.Type.Equals(AccountType.Customer))
        {
            claims.AddRange(
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserType", user.Type));
        }else if (user.Type.Equals(AccountType.Employee))
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Username));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()));
            claims.Add(new Claim("UserType", user.Type));
            claims.Add(new Claim(ClaimTypes.Role, "Manager"));
        }

        var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.Now.AddMinutes(expiresMinutes),
                signingCredentials: cre
            );
            return new JwtSecurityTokenHandler().WriteToken(token);

    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        RandomNumberGenerator.Fill(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}