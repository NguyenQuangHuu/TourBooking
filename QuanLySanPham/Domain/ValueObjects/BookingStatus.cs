using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects;

public class BookingStatus : ValueObject
{
    public static readonly BookingStatus Pending = new("ĐANG CHỜ");
    public static readonly BookingStatus Confirmed = new("ĐÃ XÁC NHẬN");
    public static readonly BookingStatus Canceled = new("ĐÃ HỦY");
    public string Value { get; private set; }

    private BookingStatus()
    {
    }

    private BookingStatus(string value)
    {
        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}