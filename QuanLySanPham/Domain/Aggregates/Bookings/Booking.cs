using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Aggregates.Bookings;

public class Booking : BaseEntity<BookingId>, IAggregateRoot
{
    public TourInstanceId TourInstanceId { get; private set; }
    public UserId  UserId { get; private set; } // customer hoặc employee book
    public Quantity Total { get; private set; }
    public BookingStatus BookingStatus { get; private set; }

    private readonly List<Passenger> _passengers = new();
    public IReadOnlyList<Passenger> Passengers => _passengers.AsReadOnly();
    private Booking(){}

    public Booking(UserId userId, TourInstanceId tourInstanceId, Quantity total)
    {
        UserId = userId;
        TourInstanceId = tourInstanceId;
        Total = total;
        BookingStatus = BookingStatus.Pending;
    }

    public void AddPassengers(List<Passenger> passenger)
    {
        _passengers.AddRange(passenger);
    }

    public void ClearPassengers()
    {
        _passengers.Clear();
    }
}