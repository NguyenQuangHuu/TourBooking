using QuanLySanPham.Domain.Aggregates.Bookings;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Interfaces;

public interface IBookingRepository
{
    Task<Booking> CreateBookingAsync(Booking booking,CancellationToken ct);
    Task UpdateBookingAsync(Booking booking, CancellationToken ct);
    Task<Booking?> GetBookingByIdAsync(BookingId id, CancellationToken ct);
    
    Task<int> AddPassengersByBookingIdAsync(BookingId id,List<Passenger> passengers,UserId userId, CancellationToken ct);
}