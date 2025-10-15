using Npgsql;
using QuanLySanPham.Domain.Interfaces;

namespace QuanLySanPham.Infrastructure.Persistence.Database;

public class DbContext : IDbContext
{
    private readonly string _connectionString;

    public DbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ??
                            throw new InvalidOperationException("Database connection string not found");
    }

    //_connectionString = "Host=localhost;Port=5432;Database=product_management;UsernameDto=postgres;PasswordDto=12345678";
    public async Task<NpgsqlConnection> CreateConnectionAsync(CancellationToken ct)
    {
        var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync(ct);
        return conn;
    }
}