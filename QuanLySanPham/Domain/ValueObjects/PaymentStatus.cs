namespace QuanLySanPham.Domain.ValueObjects;

public class PaymentStatus
{
    public static PaymentStatus Pending => new PaymentStatus("PENDING");
    public static PaymentStatus Completed => new PaymentStatus("COMPLETED");
    public static PaymentStatus Failed => new PaymentStatus("FAILED");
    public static PaymentStatus Expired => new PaymentStatus("EXPIRED");
    
    private string Value { get; set; }

    private PaymentStatus(string value)
    {
        Value = value;
    }
}