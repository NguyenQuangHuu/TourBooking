using MediatR;
using QuanLySanPham.Domain.Aggregates.Tours;
using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Infrastructure.Exceptions;

namespace QuanLySanPham.Domain.Events.Tours;

public record TourSeatsReversedEvent(TourInstance TourInstance,int Seats):IDomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
}

public class TourSeatsReversedEventHandler : INotificationHandler<TourSeatsReversedEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITourManagementRepository _tourManagementRepository;
    private readonly ILogger<TourSeatsReversedEvent> _logger;

    public TourSeatsReversedEventHandler(IUnitOfWork unitOfWork, ITourManagementRepository tourManagementRepository, ILogger<TourSeatsReversedEvent> logger)
    {
        _unitOfWork = unitOfWork;
        _tourManagementRepository = tourManagementRepository;
        _logger = logger;
    }
    public async Task Handle(TourSeatsReversedEvent notification, CancellationToken ct)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(ct);
            var tourInstance = notification.TourInstance;
            tourInstance.SeatReverse(notification.Seats);
            var result = await _tourManagementRepository.UpdateTourInstanceAsync(tourInstance, ct);
            if (result > 0)
            {
                _logger.LogInformation($"Đặt chỗ thành công cho {tourInstance.Id} với {notification.Seats} người!");
            }
            else
            {
                _logger.LogError("Có lỗi xảy ra khi cập nhật số lượng chỗ!");
            }
        }
        catch (InfrastructureException ex)
        {
            await _unitOfWork.RollbackAsync(ct);
            _logger.LogError(ex, ex.Message);
        }

    }
}