using Npgsql;
using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Infrastructure.Interfaces;

namespace QuanLySanPham.Infrastructure.Commons;

public class DbCommandDefinition : IDbCommandDefinition
{
    public string Sql { get; }
    public object? Parameters { get; }
    public Func<NpgsqlCommand, Task<object?>>? Executor { get; }

    public DbCommandDefinition(string sql, object? parameters, Func<NpgsqlCommand, Task<object?>>? executor)
    {
        Sql = sql;
        Parameters = parameters;
        Executor = executor;
    }
}