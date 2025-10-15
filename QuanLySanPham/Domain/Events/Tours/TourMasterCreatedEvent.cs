using MediatR;
using QuanLySanPham.Domain.Aggregates.Tours;
using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Domain.Events.Tours;

public class TourMasterCreatedEvent : IDomainEvent
{
    public Guid EventId { get; set; }
    public DateTime OccurredOn { get; set; }
    public TourMasterId TourMasterId { get; set; }

    public List<TourMasterDestination> TourMasterDestinations { get; set; }

    public List<TourMasterPoi> TourMasterPois { get; set; }

    public TourMasterCreatedEvent(TourMasterId tourMasterId, List<TourMasterDestination> tourMasterDestinations,
        List<TourMasterPoi> tourMasterPoi)
    {
        TourMasterId = tourMasterId;
        OccurredOn = DateTime.Now;
        EventId = Guid.NewGuid();
        TourMasterDestinations = tourMasterDestinations;
        TourMasterPois = tourMasterPoi;
    }
}

public class TourMasterCreatedEventHandler : INotificationHandler<TourMasterCreatedEvent>
{
    public async Task Handle(TourMasterCreatedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(notification.TourMasterId);
        await Task.CompletedTask;
    }
}