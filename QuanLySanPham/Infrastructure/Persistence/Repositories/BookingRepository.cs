using Npgsql;
using QuanLySanPham.Domain.Aggregates.Bookings;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Infrastructure.Exceptions;

namespace QuanLySanPham.Infrastructure.Persistence.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public BookingRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Booking> CreateBookingAsync(Booking booking, CancellationToken ct)
    {
        string sql =
            "insert into bookings(total_amount, booking_status,tour_instance_id,user_id) values(@TotalSlots,@BookingStatus,@TourInstanceId,@UserId) returning id ";

        await using NpgsqlCommand cmd =
            new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        cmd.Parameters.Add(new NpgsqlParameter("@TotalSlots", booking.Total.Value));
        cmd.Parameters.Add(new NpgsqlParameter("@BookingStatus", booking.BookingStatus.Value));
        cmd.Parameters.Add(new NpgsqlParameter("@TourInstanceId", booking.TourInstanceId.Value));
        cmd.Parameters.Add(new NpgsqlParameter("@UserId", booking.UserId.Value));
        object? result = await cmd.ExecuteScalarAsync(ct);
        if (result is null)
        {
            throw new InfrastructureException("Cannot insert");
        }
        BookingId bookingId  = BookingId.From((Guid)result);
        booking.Id = bookingId;
        return booking;
    }
}