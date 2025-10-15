using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Domain.Commons;

public abstract class BaseEntity<TId> : IEntity where TId : EntityId
{
    public TId Id { get; set; }
    EntityId IEntity.Id => Id;
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected BaseEntity()
    {
    }

    protected BaseEntity(TId id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}