using MediatR;
using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Domain.Events.Destinations;

public record DestinationCreatedEvent(DestinationId Id) : IDomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
}

public class DestinationCreatedEventHandler : INotificationHandler<DestinationCreatedEvent>
{
    private readonly IMediator _mediator;

    public DestinationCreatedEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(DestinationCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Publish(notification.Id, cancellationToken);
    }
}