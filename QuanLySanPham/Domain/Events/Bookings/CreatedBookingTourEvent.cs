using MediatR;
using QuanLySanPham.Domain.Aggregates.Bookings;
using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.Interfaces;

namespace QuanLySanPham.Domain.Events.Bookings;

public record CreatedBookingTourEvent(Booking Booking) : IDomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
}

public class CreatedBookingTourEventHandler : INotificationHandler<CreatedBookingTourEvent>
{
    private readonly ILogger<CreatedBookingTourEventHandler> _logger;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatedBookingTourEventHandler(ILogger<CreatedBookingTourEventHandler> logger, IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreatedBookingTourEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreatedBookingTourEvent)} - {notification.Booking.UserId} - Đã đặt tour - {notification.Booking.Id}");
        await Task.CompletedTask;
    }
}