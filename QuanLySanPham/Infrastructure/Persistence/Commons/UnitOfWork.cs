using MediatR;
using Npgsql;
using QuanLySanPham.Application.Services;
using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.Interfaces;

namespace QuanLySanPham.Infrastructure.Persistence.Commons;

public class UnitOfWork : IUnitOfWork,ITrackedEntities,IDomainEventDispatcher
{
    private readonly ILogger<UnitOfWork> _logger;
    private readonly IDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly List<IEntity> _baseEntities = new();
    public NpgsqlConnection Connection { get; private set; }
    public NpgsqlTransaction Transaction { get; private set; }

    private UnitOfWork()
    {
    }

    public UnitOfWork(IDbContext dbContext, IMediator mediator, ILogger<UnitOfWork> logger)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task BeginTransactionAsync(CancellationToken token)
    {
        Connection = await _dbContext.CreateConnectionAsync(token);
        Transaction = await Connection.BeginTransactionAsync(token);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await Transaction.CommitAsync(cancellationToken);
        await DispatchEventAsync(cancellationToken);
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

    public void TrackEntity(IEntity entity)
    {
        _baseEntities.Add(entity);
    }

    public IReadOnlyList<IEntity> GetTrackedEntities => _baseEntities.AsReadOnly();
    public async Task DispatchEventAsync(CancellationToken ct)
    {
        var domain = GetTrackedEntities.SelectMany(e => e.DomainEvents).ToList();
        foreach (var domainEvent in domain)
        {
            _logger.LogInformation("Dispatching domain event {domainEvent}", domainEvent.GetType().Name);
            await _mediator.Publish(domainEvent,ct);
        }
        _baseEntities.ForEach(x => x.ClearDomainEvents());
    }
}