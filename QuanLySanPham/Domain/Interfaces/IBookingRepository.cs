using QuanLySanPham.Domain.Aggregates.Bookings;

namespace QuanLySanPham.Domain.Interfaces;

public interface IBookingRepository
{
    Task<Booking> CreateBookingAsync(Booking booking,CancellationToken ct);
}