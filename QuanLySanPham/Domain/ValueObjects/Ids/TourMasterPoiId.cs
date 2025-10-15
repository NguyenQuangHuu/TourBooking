using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects.Ids;

public class TourMasterPoiId : EntityId
{
    public TourMasterPoiId(Guid value) : base(value)
    {
    }

    public static TourMasterPoiId From(Guid id)
    {
        return new TourMasterPoiId(id);
    }
}