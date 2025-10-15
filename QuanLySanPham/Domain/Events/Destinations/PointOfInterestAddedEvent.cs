using MediatR;
using QuanLySanPham.Domain.Aggregates.Destinations;
using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.Events.Destinations;

public record PointOfInterestAddedEvent(PointOfInterest poi) : IDomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
}

public class PointOfInterestAddedEventHandler(IMediator mediator) : INotificationHandler<PointOfInterestAddedEvent>
{
    public async Task Handle(PointOfInterestAddedEvent notification, CancellationToken cancellationToken)
    {
        await mediator.Publish(notification.poi.Id, cancellationToken);
    }
}