using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects;

public class PointOfInterestId : EntityId
{
    public PointOfInterestId(Guid value) : base(value)
    {
    }

    public static PointOfInterestId From(Guid value)
    {
        return new PointOfInterestId(value);
    }
}