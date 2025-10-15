using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Aggregates.Tours;

public class TourMasterDestination : BaseEntity<TourMasterDestinationId>
{
    public TourMasterId TourMasterId { get; set; }
    public DestinationId DestinationId { get; set; }
    public int Order { get; set; }
    public int StayDays { get; set; }

    public TourMasterDestination(DestinationId destinationId, int order, int stayDays)
    {
        DestinationId = destinationId;
        Order = order;
        StayDays = stayDays;
    }
}