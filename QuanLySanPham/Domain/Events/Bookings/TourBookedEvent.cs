using MediatR;
using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Events.Bookings;

public record TourBookedEvent(BookingId BookingId) : IDomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
}

public class TourBookedEventHandler : INotificationHandler<TourBookedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<TourBookedEventHandler> _logger;

    public TourBookedEventHandler(IMediator mediator, ILogger<TourBookedEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task Handle(TourBookedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("TourBookedEvent received, id: {id}", notification.EventId);
        await Task.CompletedTask;
    }
}
