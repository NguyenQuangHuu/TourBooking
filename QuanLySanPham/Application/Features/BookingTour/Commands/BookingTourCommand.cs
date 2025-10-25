using MediatR;
using QuanLySanPham.Application.Exceptions;
using QuanLySanPham.Domain.Aggregates.Bookings;
using QuanLySanPham.Domain.Aggregates.Tours;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Infrastructure.Exceptions;
using QuanLySanPham.Presentations.DTOs.Responses;

namespace QuanLySanPham.Application.Features.BookingTour.Commands;

public record BookingTourCommand(UserId UserId,TourInstanceId TourInstanceId,int TotalSlots) : IRequest<Result<Booking>>
{
    
}

public class BookingTourCommandHandler : IRequestHandler<BookingTourCommand, Result<Booking>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookingRepository _bookingRepository;
    private readonly ITourManagementRepository _tourManagementRepository;

    public BookingTourCommandHandler(IUnitOfWork unitOfWork, IBookingRepository bookingRepository, ITourManagementRepository tourManagementRepository)
    {
        _unitOfWork = unitOfWork;
        _bookingRepository = bookingRepository;
        _tourManagementRepository = tourManagementRepository;
    }

    public async Task<Result<Booking>> Handle(BookingTourCommand request, CancellationToken ct)
    {
        try
        {
            await _unitOfWork.BeginAsync(ct);
            var tour = await _tourManagementRepository.GetTourInstanceByIdAsync(request.TourInstanceId, ct);
            if (tour is null)
            {
                return Result<Booking>.Failure("TourInstance does not exist", StatusCodes.Status404NotFound);
            }

            tour.UpdateAvailableSlot(request.TotalSlots);
            await _tourManagementRepository.UpdateTourInstanceAsync(tour, ct);
            var booking = new Booking(request.UserId, request.TourInstanceId, new Quantity(request.TotalSlots));
            var createBooking = await _bookingRepository.CreateBookingAsync(booking, ct);
            await _unitOfWork.CommitAsync(ct);
            return Result<Booking>.Success(createBooking, StatusCodes.Status201Created);
        }
        catch (DomainException ex)
        {
            return Result<Booking>.Failure(ex.Message, StatusCodes.Status400BadRequest);
        }
        catch (InfrastructureException ex)
        {
            return Result<Booking>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}