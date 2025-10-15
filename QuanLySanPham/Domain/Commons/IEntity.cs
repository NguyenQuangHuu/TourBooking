using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Domain.Commons;

public interface IEntity
{
    EntityId Id { get; }
    DateTime CreatedAt { get; }
    DateTime? ModifiedAt { get; }
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    void AddDomainEvent(IDomainEvent eventItem);
    void ClearDomainEvents();
}