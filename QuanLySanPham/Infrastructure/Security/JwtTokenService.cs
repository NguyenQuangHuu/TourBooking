using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using QuanLySanPham.Application.Services;
using QuanLySanPham.Domain.Aggregates.Auth;
using QuanLySanPham.Domain.Aggregates.Employees;
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

    public string GenerateJwtTokenForCustomer(User user)
    {
        var key = _configuration["Jwt:Key"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expiresMinutes = int.Parse(_configuration["Jwt:ExpiresMinutes"] ?? "20");

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var cre = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>();
        var iatEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        claims.AddRange(
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, iatEpoch, ClaimValueTypes.Integer64),
                new Claim("UserType", user.Type));
        var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.Now.AddMinutes(expiresMinutes),
                signingCredentials: cre
            );
            return new JwtSecurityTokenHandler().WriteToken(token);

    }

    public string GenerateJwtTokenForEmployee(User user, Employee emp)
    {       
        var key = _configuration["Jwt:Key"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expiresMinutes = int.Parse(_configuration["Jwt:ExpiresMinutes"] ?? "20");

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var cre = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>();
        var iatEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        claims.AddRange(
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, iatEpoch, ClaimValueTypes.Integer64),
            new Claim("UserType", user.Type));
        if (emp.Role.Value is null)
        {
                throw new ArgumentNullException(nameof(emp)); 
        }
        claims.Add(new Claim(ClaimTypes.Role,emp.Role.Value));
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