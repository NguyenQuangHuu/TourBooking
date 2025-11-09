using MediatR;
using QuanLySanPham.Application.Services;
using QuanLySanPham.Domain.Aggregates.Bookings;
using QuanLySanPham.Domain.Events.Bookings;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Presentations.DTOs.DTO;
using QuanLySanPham.Presentations.DTOs.Responses;

namespace QuanLySanPham.Application.Features.BookingTour.Commands;

public record AddPassengerCommand(UserId UserId,BookingId BookingId, List<PassengerDto> Passengers) : IRequest<Result<BookingId>>
{
    
}

public class AddPassengerCommandHandler : IRequestHandler<AddPassengerCommand, Result<BookingId>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookingRepository _bookingRepository;
    private readonly ITrackedEntities _trackedEntities;

    public AddPassengerCommandHandler(IUnitOfWork unitOfWork, IBookingRepository bookingRepository)
    {
        _unitOfWork = unitOfWork;
        _bookingRepository = bookingRepository;
    }
    public async Task<Result<BookingId>> Handle(AddPassengerCommand request, CancellationToken ct)
    {
        await _unitOfWork.BeginTransactionAsync(ct);
        var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId, ct);
        if (booking is null)
        {
            return Result<BookingId>.Failure("Không tìm thấy mã đặt chỗ",404);
        }

        if (booking.UserId != request.UserId)
        {
            return Result<BookingId>.Failure("Tài khoản không khớp với hóa đơn!",StatusCodes.Status400BadRequest);
        }

        if (booking.TotalSlots.Value != request.Passengers.Count)
        {
            return Result<BookingId>.Failure("Lượng đặt chỗ không khớp với số lượng khách hiện tại!",StatusCodes.Status400BadRequest);
        }

        List<Passenger> passengers = new();
        foreach (var passengerDto in request.Passengers)
        {
            var passenger = new Passenger(new PassengerInfo(passengerDto.FullName,passengerDto.BirthDate,(Gender)passengerDto.Gender,passengerDto.IdentityNo,passengerDto.PassportNo),new PassengerContact(passengerDto.PhoneNumber,passengerDto.Email,passengerDto.Address),request.BookingId);
            passengers.Add(passenger);
        }
        var result = await _bookingRepository.AddPassengersByBookingIdAsync(request.BookingId,passengers,request.UserId,ct);
        if (result > 0)
        {
            booking.BookingStatus = BookingStatus.Confirmed;
            await _bookingRepository.UpdateBookingAsync(booking, ct);
            booking.AddDomainEvent(new AddedPassengerEvent(booking));
            _trackedEntities.TrackEntity(booking);
            await _unitOfWork.CommitAsync(ct);
            return Result<BookingId>.Success(request.BookingId,StatusCodes.Status201Created);
        }
        await _unitOfWork.RollbackAsync(ct);
        return Result<BookingId>.Failure("Cập nhật hành khách không thành công",StatusCodes.Status400BadRequest);
    }
}