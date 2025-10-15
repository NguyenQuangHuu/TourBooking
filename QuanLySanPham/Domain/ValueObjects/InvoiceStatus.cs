using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects;

public class InvoiceStatus : ValueObject
{
    public static readonly InvoiceStatus Unpaid = new("CHƯA THANH TOÁN");
    public static readonly InvoiceStatus Paid = new("ĐÃ THANH TOÁN");
    public static readonly InvoiceStatus Canceled = new("ĐÃ HỦY");

    public string Value { get; private set; }

    private InvoiceStatus()
    {
    }

    private InvoiceStatus(string value)
    {
        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}