using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Domain.Aggregates.Destinations;

public class PointOfInterest : BaseEntity<PointOfInterestId>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public PoiType PoiType { get; set; }
    public TimeSchedule Duration { get; set; }
    public DestinationId DestinationId { get; set; }

    public PointOfInterest()
    {
    }

    public PointOfInterest(string name, string description, PoiType poiType, TimeSchedule duration,
        DestinationId destinationId)
    {
        Name = name;
        Description = description;
        PoiType = poiType;
        Duration = duration;
        DestinationId = destinationId;
    }

    public void UpdatePoi(string name, string description, PoiType poiType, TimeSchedule duration)
    {
        Name = name;
        Description = description;
        PoiType = poiType;
        Duration = duration;
    }
}