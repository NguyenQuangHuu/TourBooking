using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Aggregates.Bookings;

public class Passenger : BaseEntity<PassengerId>
{
    public PassengerInfo PassengerInfo { get; set; }
    public PassengerContact PassengerContact { get; set; }
    public BookingId BookingId { get;private set; }

    public Passenger(PassengerInfo passengerInfo, PassengerContact passengerContact, BookingId bookingId)
    {
        PassengerInfo = passengerInfo;
        PassengerContact = passengerContact;
        BookingId = bookingId;
    }

    public void UpdateInformation(PassengerInfo passengerInfo)
    {
        PassengerInfo = passengerInfo;
    }

    public void UpdateContact(PassengerContact passengerContact)
    {
        PassengerContact = passengerContact;
    }
}