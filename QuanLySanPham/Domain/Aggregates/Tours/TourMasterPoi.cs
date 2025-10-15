using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Aggregates.Tours;

public class TourMasterPoi : BaseEntity<TourMasterPoiId>
{
    public TourMasterId TourMasterId { get; set; }
    public PointOfInterestId PointOfInterestId { get; set; }
    public int Order { get; set; }

    public TourMasterPoi(PointOfInterestId pointOfInterestId, int order)
    {
        PointOfInterestId = pointOfInterestId;
        Order = order;
    }
}