using System.Runtime.InteropServices.ComTypes;
using MediatR;
using QuanLySanPham.Domain.Aggregates.Bookings;
using QuanLySanPham.Domain.Aggregates.Invoices;
using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Events.Bookings;

public record AddedPassengerEvent(Booking Booking) : IDomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
}

public class AddedPassengerEventHandler : INotificationHandler<AddedPassengerEvent>
{
    private readonly ILogger<AddedPassengerEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IInvoiceRepository _invoiceRepository;
    public AddedPassengerEventHandler(ILogger<AddedPassengerEventHandler> logger, IUnitOfWork  unitOfWork, IInvoiceRepository invoiceRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _invoiceRepository = invoiceRepository;
    }
    
    public async Task Handle(AddedPassengerEvent notification, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginAsync(cancellationToken);
        var invoice = new Invoice(notification.Booking.Id, notification.Booking.UserId,notification.Booking.TotalAmount);
        var result = await _invoiceRepository.CreateInvoiceAsync(invoice,cancellationToken);
        _logger.LogInformation($"Tạo hóa đơn thành công : {result.Id}!");
        await Task.CompletedTask;
    }
}
