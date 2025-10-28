using MediatR;

namespace QuanLySanPham.Domain.Commons;

public interface IDomainEvent : INotification
{
    Guid EventId { get; } 
    DateTime OccurredOn { get; }
}