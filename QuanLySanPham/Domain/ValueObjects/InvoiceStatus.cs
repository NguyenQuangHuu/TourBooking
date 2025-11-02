using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.Exceptions;

namespace QuanLySanPham.Domain.ValueObjects;

public class InvoiceStatus : ValueObject
{
    public static readonly InvoiceStatus Unpaid = new("CHƯA THANH TOÁN");
    public static readonly InvoiceStatus Paid = new("ĐÃ THANH TOÁN");
    public static readonly InvoiceStatus Canceled = new("ĐÃ HỦY");
    public static readonly InvoiceStatus Refunded = new("ĐÃ HOÀN TIỀN");

    public string Value { get; private set; }

    private InvoiceStatus()
    {
    }

    private InvoiceStatus(string value)
    {
        Value = value;
    }
    

    public bool CanChangeTo(InvoiceStatus newStatus)
    {
        return (Value, newStatus.Value) switch
        {
             ("CHƯA THANH TOÁN","ĐÃ THANH TOÁN") => true,
             ("CHƯA THANH TOÁN","ĐÃ HỦY")=>true,
             //("CHƯA THANH TOÁN",_)=>false,
             ("ĐÃ THANH TOÁN","ĐÃ HOÀN TIỀN")=>true,
             //("ĐÃ THANH TOÁN",_)=>false,
             //("ĐÃ HỦY",_)=>false,
             //("ĐÃ HOÀN TIỀN",_)=>false,
             _=>false
        };
    }
    
    /// <summary>
    ///     Hàm xây dựng để lưu dữ liệu khi lấy từ database adonet lên
    /// </summary>
    /// <param name="invoiceStatus">trạng thái của hóa đơn</param>
    /// <returns>Đối tượng InvoiceStatus</returns>
    /// <exception cref="DomainException"></exception>
    public static InvoiceStatus From(string invoiceStatus)
    {
        return invoiceStatus.ToUpperInvariant() switch
        {
            "CHƯA THANH TOÁN" => Unpaid,
            "ĐÃ THANH TOÁN" => Paid,
            "ĐÃ HỦY" => Canceled,
            "ĐÃ HOÀN TIỀN" => Refunded,
            _ => throw new DomainException("Trạng thái không hợp lệ!")
        };
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}