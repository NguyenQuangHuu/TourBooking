using MediatR;
using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Domain.Events.Tours;

public record UpdatedTourInstanceEvent: IDomainEvent
{
    public TourInstanceId TourInstanceId { get; set; }
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }

    public UpdatedTourInstanceEvent(TourInstanceId tourInstanceId)
    {
        TourInstanceId = tourInstanceId;
    }
}

public class UpdatedTourInstanceEventHandler : INotificationHandler<UpdatedTourInstanceEvent>
{
    private readonly ILogger _logger;
    private readonly IMediator  _mediator;
    public UpdatedTourInstanceEventHandler(IMediator mediator, ILogger<UpdatedTourInstanceEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public Task Handle(UpdatedTourInstanceEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UpdatedTourInstanceEvent trigger, tour instance {id} has been updated ", notification.TourInstanceId);
        return Task.CompletedTask;
    }
}