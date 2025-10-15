using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects;

public class TourMasterId : EntityId
{
    public TourMasterId(Guid value) : base(value)
    {
    }

    public static TourMasterId From(Guid id)
    {
        return new TourMasterId(id);
    }
}