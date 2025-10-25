using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.Exceptions;

namespace QuanLySanPham.Domain.ValueObjects;

public class BookingStatus : ValueObject
{
    public static readonly BookingStatus Pending = new("ĐANG CHỜ");
    public static readonly BookingStatus Confirmed = new("ĐÃ XÁC NHẬN");
    public static readonly BookingStatus Canceled = new("ĐÃ HỦY");
    private static readonly List<BookingStatus> BookingStatuses = new(){Pending,Confirmed,Canceled};

    public string Value { get; set; }

    private BookingStatus()
    {
    }

    private BookingStatus(string status)
    {
        Value = status;
    }

    public static implicit operator string(BookingStatus bookingStatus) => bookingStatus.Value;

    public bool ValidStatusChange(BookingStatus  bookingStatus)
    {
        return (this, bookingStatus) switch
        {
            (var current, var next) when current == Pending && (next == Confirmed || next == Canceled) => true,
            (var current,var next) when current == Confirmed && (next == Canceled) => true,
            (var current,_) when current == Canceled => false
        };
    }
    public static BookingStatus From(string value)
        => BookingStatuses.SingleOrDefault(s => s.Value == value)
           ?? throw new DomainException($"Trạng thái '{value}' không hợp lệ");
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}