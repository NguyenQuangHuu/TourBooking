using QuanLySanPham.Domain.Aggregates.Auth;

namespace QuanLySanPham.Application.Services;

public interface IJwtTokenService
{
    string GenerateJwtToken(User user);

    string GenerateRefreshToken();
}