using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects.Ids;

public class TourMasterDestinationId : EntityId
{
    public TourMasterDestinationId(Guid value) : base(value)
    {
    }

    public static TourMasterDestinationId From(Guid id)
    {
        return new TourMasterDestinationId(id);
    }
}