using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects.Ids;

public class PassengerId : EntityId
{
    public Guid Value { get;private set; }
    public PassengerId(Guid value) : base(value)
    {
    }

    public static implicit operator Guid(PassengerId passengerId) => passengerId.Value;
    public static explicit operator PassengerId(Guid passengerId) => new PassengerId(passengerId);
}