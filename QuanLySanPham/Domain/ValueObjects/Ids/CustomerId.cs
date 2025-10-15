using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects.Ids;

public class CustomerId : EntityId
{
    public CustomerId(Guid value) : base(value)
    {
    }

    public static CustomerId From(Guid id)
    {
        return new CustomerId(id);
    }
}