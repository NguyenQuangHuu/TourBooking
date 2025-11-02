using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Aggregates.Bookings;

public class Booking : BaseEntity<BookingId>, IAggregateRoot
{
    public TourInstanceId TourInstanceId { get; set; }
    public UserId  UserId { get; set; } // customer hoặc employee book
    public Quantity TotalSlots { get; set; }
    public BookingStatus BookingStatus { get; set; }
    
    public Money TotalAmount { get; set; }

    private readonly List<Passenger> _passengers = new();
    public IReadOnlyList<Passenger> Passengers => _passengers.AsReadOnly();
    public Booking(){}

    public Booking(UserId userId, TourInstanceId tourInstanceId, Quantity totalSlots,Money pricePerPax)
    {
        UserId = userId;
        TourInstanceId = tourInstanceId;
        TotalSlots = totalSlots;
        BookingStatus = BookingStatus.Pending;
        TotalAmount = new Money(TotalSlots.Value * pricePerPax.Amount);
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