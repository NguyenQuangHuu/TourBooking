using MediatR;
using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Domain.Events.Tours;

public class TourInstanceCreatedEvent : IDomainEvent
{
    public Guid EventId { get; set; }
    public DateTime OccurredOn { get; set; }

    public TourInstanceId TourInstanceId { get; set; }

    public TourInstanceCreatedEvent(TourInstanceId tourInstanceId)
    {
        OccurredOn = DateTime.Now;
        TourInstanceId = tourInstanceId;
        EventId = Guid.NewGuid();
    }
}

public class TourInstanceCreatedEventHandler(IMediator mediator) : INotificationHandler<TourInstanceCreatedEvent>
{
    public async Task Handle(TourInstanceCreatedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine("Tour Instance created");
        await Task.CompletedTask;
    }
}