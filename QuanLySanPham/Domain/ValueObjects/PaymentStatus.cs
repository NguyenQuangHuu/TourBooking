using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.Exceptions;

namespace QuanLySanPham.Domain.ValueObjects;

public class PaymentStatus : ValueObject
{
    public static PaymentStatus Pending => new PaymentStatus("CHỜ THANH TOÁN");
    public static PaymentStatus Completed => new PaymentStatus("HOÀN TẤT THANH TOÁN");
    public static PaymentStatus Failed => new PaymentStatus("THANH TOÁN THẤT BẠI");
    public static PaymentStatus Expired => new PaymentStatus("QUÁ HẠN THANH TOÁN");
    
    private string Value { get;}

    private PaymentStatus(string value)
    {
        Value = value;
    }

    private bool ValidStatusChange(PaymentStatus paymentStatus)
    {
        return (Value, paymentStatus.Value) switch
        {
            ("CHỜ THANH TOÁN","HOÀN TẤT THANH TOÁN") => true,
            ("CHỜ THANH TOÁN","THANH TOÁN THẤT BẠI") => true,
            ("CHỜ THANH TOÁN","QUÁ HẠN THANH TOÁN") => true,
            ("THANH TOÁN THẤT BẠI","QUÁ HẠN THANH TOÁN")=>true,
            _ => false
        };
    }
    public PaymentStatus ChangePaymentStatus(PaymentStatus paymentStatus)
    {
        if (ValidStatusChange(paymentStatus))
        {
            return paymentStatus;
        }
        else
        {
            throw new DomainException("Trạng thái không hợp lệ");
        }
    }
    
    public static PaymentStatus From(string value)
    {
        return value switch
        {
            "CHỜ THANH TOÁN" => Pending,
            "HOÀN TẤT THANH TOÁN" => Completed,
            "THANH TOÁN THẤT BẠI" => Failed,
            "QUÁ HẠN THANH TOÁN" => Expired,
            _ => throw new DomainException("Trạng thái không hợp lệ")
        };
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}