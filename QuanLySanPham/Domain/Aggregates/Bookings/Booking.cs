using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Aggregates.Bookings;

public class Booking : BaseEntity<BookingId>
{
    public TourInstanceId TourInstanceId { get; private set; }
    public BookingInfo BookingInfo { get; set; }
}