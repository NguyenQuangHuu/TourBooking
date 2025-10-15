using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects;

public class TourInstanceId : EntityId
{
    public TourInstanceId(Guid value) : base(value)
    {
    }

    public static TourInstanceId From(Guid id)
    {
        return new TourInstanceId(id);
    }
}