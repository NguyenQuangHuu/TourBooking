using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects.Ids;

public class RoleId : EntityId
{
    public RoleId(Guid value) : base(value)
    {
    }
    public static implicit operator Guid(RoleId id) => id.Value;
    public static explicit operator RoleId(Guid guid) => new RoleId(guid);
}