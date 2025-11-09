using Npgsql;
using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Domain.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    NpgsqlConnection Connection { get; }
    NpgsqlTransaction Transaction { get; }
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
}