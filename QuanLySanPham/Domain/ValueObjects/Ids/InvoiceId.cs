using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects.Ids;

public class InvoiceId : EntityId
{
    public Guid Value { get; set; }
    public InvoiceId(Guid value) : base(value)
    {
    }
    public static implicit operator Guid(InvoiceId instance)=> instance.Value;
    public static explicit operator InvoiceId(Guid instance) => new InvoiceId(instance);
}