using MediatR;
using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Events.Payments;

public record PaymentCreatedEvent(PaymentId PaymentId,InvoiceId InvoiceId) : IDomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
}

public class PaymentCreatedEventHandler : INotificationHandler<PaymentCreatedEvent>
{
    private readonly ILogger<PaymentCreatedEventHandler> _logger;

    public PaymentCreatedEventHandler(ILogger<PaymentCreatedEventHandler> logger)
    {
        _logger = logger;
    }
    public async Task Handle(PaymentCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Created Payment:{notification.PaymentId} for Invoice:{notification.InvoiceId} successfully");
        await Task.CompletedTask;
    }
}