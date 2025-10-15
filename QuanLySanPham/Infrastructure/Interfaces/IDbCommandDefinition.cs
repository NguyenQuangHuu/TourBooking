using Npgsql;
using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Infrastructure.Interfaces;

public interface IDbCommandDefinition
{
    string Sql { get; }
    object? Parameters { get; }
    Func<NpgsqlCommand, Task<object?>>? Executor { get; }
}