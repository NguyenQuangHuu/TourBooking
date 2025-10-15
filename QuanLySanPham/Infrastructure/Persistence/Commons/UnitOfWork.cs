using Npgsql;
using QuanLySanPham.Domain.Interfaces;

namespace QuanLySanPham.Infrastructure.Persistence.Commons;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbContext _dbContext;
    public NpgsqlConnection Connection { get; private set; } = null!;
    public NpgsqlTransaction Transaction { get; private set; } = null!;

    private UnitOfWork()
    {
    }

    public UnitOfWork(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task BeginAsync(CancellationToken token)
    {
        Connection = await _dbContext.CreateConnectionAsync(token);
        Transaction = await Connection.BeginTransactionAsync(token);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await Transaction.CommitAsync(cancellationToken);
        await Connection.CloseAsync();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        await Transaction.RollbackAsync(cancellationToken);
        await Connection.CloseAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await Transaction.DisposeAsync();
        await Connection.DisposeAsync();
    }
}