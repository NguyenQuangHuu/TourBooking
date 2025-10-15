using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects.Ids;

public class UserId : EntityId
{
    public UserId(Guid value) : base(value)
    {
    }

    public static UserId From(Guid id)
    {
        return new UserId(id);
    }
}