using Npgsql;

namespace QuanLySanPham.Domain.Interfaces;

public interface IDbContext
{
    Task<NpgsqlConnection> CreateConnectionAsync(CancellationToken ct);
}