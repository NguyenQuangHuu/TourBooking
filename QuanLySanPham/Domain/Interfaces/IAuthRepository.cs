using QuanLySanPham.Domain.Aggregates.Auth;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Infrastructure.Interfaces;

namespace QuanLySanPham.Domain.Interfaces;

public interface IAuthRepository
{
    Task<UserId?> CreateUserAsync(User user, CancellationToken ct);
    Task<User?> GetUserByUsernameAsync(string username, CancellationToken ct);

    Task SaveRefreshTokenAsync(string token, DateTime expiresAt, UserId userId, CancellationToken ct);

    Task<User?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken ct);
}