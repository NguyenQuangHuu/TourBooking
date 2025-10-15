using Npgsql;
using QuanLySanPham.Domain.Aggregates.Auth;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Infrastructure.Commons;
using QuanLySanPham.Infrastructure.Interfaces;

namespace QuanLySanPham.Infrastructure.Persistence.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserId?> CreateUserAsync(User user, CancellationToken ct)
    {
        var sql =
            @"insert into users(username,password,email,phone_number,user_type) values(@Username,@Password,@Email,@PhoneNumber,@UserType) returning id";

        await using var command = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        command.Parameters.Add(new NpgsqlParameter("@Username", user.Username));
        command.Parameters.Add(new NpgsqlParameter("@Password", user.PasswordHash));
        command.Parameters.Add(new NpgsqlParameter("@Email", user.Email));
        command.Parameters.Add(new NpgsqlParameter("@PhoneNumber", user.PhoneNumber));
        command.Parameters.Add(new NpgsqlParameter("@UserType", user.Type.ToString()));
        var result = await command.ExecuteScalarAsync(ct);
        if (result is not null) return UserId.From((Guid)result);
        return null;
    }


    public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken ct)
    {
        var sql = "select * from users where username = @Username";
        await using var command = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        command.Parameters.Add(new NpgsqlParameter("@Username", username));
        await using var reader = await command.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
            return new User
            {
                Id = UserId.From(reader.GetGuid(0)),
                Username = reader.GetString(3),
                PasswordHash = reader.GetString(4),
                Email = reader.GetString(5),
                PhoneNumber = reader.GetString(6),
                Type = (AccountType)reader.GetString(7)
            };
        return null;
    }

    public async Task UpdateUserAsync(User user, CancellationToken ct)
    {
        var sql = "update users set password = @Password,email = @Email,phone_number = @PhoneNumber, refresh_token = @RfToken,rf_token_expire = @ExpiresAt where id = @Id";
        await using var command = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        command.Parameters.Add(new NpgsqlParameter("@Password", user.PasswordHash));
        command.Parameters.Add(new NpgsqlParameter("@Email", user.Email));
        command.Parameters.Add(new NpgsqlParameter("@PhoneNumber", user.PhoneNumber));
        command.Parameters.Add(new NpgsqlParameter("@RfToken", user.RefreshToken));
        command.Parameters.Add(new NpgsqlParameter("@ExpiresAt", user.RefreshTokenExpiration));
        command.Parameters.Add(new NpgsqlParameter("@Id", user.Id.Value));
        await command.ExecuteNonQueryAsync(ct);
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken ct)
    {
        var sql =
            "select id,username,password,email,phone_number,user_type,refresh_token,rf_token_expire from users where refresh_token = @RfToken";
        await using var command = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        command.Parameters.Add(new NpgsqlParameter("@RfToken", refreshToken));
        await using var reader = await command.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
            return new User
            {
                Id = UserId.From(reader.GetGuid(0)),
                Username = reader.GetString(1),
                PasswordHash = reader.GetString(2),
                Email = reader.GetString(3),
                PhoneNumber = reader.GetString(4),
                Type = (AccountType)reader.GetString(5),
                RefreshToken = reader.GetString(6),
                RefreshTokenExpiration = reader.GetDateTime(7)
            };

        return null;
    }
}