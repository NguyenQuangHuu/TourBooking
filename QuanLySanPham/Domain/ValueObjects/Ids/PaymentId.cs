using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects.Ids;

public class PaymentId : EntityId
{
    public Guid Value;
    public PaymentId(Guid value) : base(value)
    {
        Value = value;
    }
    public static implicit operator Guid(PaymentId  paymentId)
    {
        return paymentId.Value;
    }
    
    public static explicit operator PaymentId(Guid guid)=> new(guid);
}