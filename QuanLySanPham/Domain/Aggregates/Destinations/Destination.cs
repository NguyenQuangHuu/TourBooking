using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.Events.Destinations;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Domain.Aggregates.Destinations;

public class Destination : BaseEntity<DestinationId>, IAggregateRoot
{
    public string Name { get; set; }

    public string Country { get; set; }

    public bool IsOverSea { get; set; }

    private readonly List<PointOfInterest> _pointOfInterests = new();
    public IReadOnlyList<PointOfInterest> PointOfInterests => _pointOfInterests;

    public Destination()
    {
    }

    public Destination(string name, string country, bool isOverSea)
    {
        Id = DestinationId.CreateId;
        Name = name;
        Country = country;
        IsOverSea = isOverSea;
    }

    public void AddPoi(PointOfInterest poi)
    {
        _pointOfInterests.Add(poi);
    }

    public PointOfInterest? CreatePointOfInterest(string name, string description, string poiType, double duration)
    {
        try
        {
            var type = PoiType.From(poiType);
            var time = new TimeSchedule(duration);
            var poi = new PointOfInterest(name, description, type, time, Id);
            _pointOfInterests.Add(poi);
            AddDomainEvent(new PointOfInterestAddedEvent(poi));
            Console.WriteLine("Add POI Success!");
            return poi;
        }
        catch (DomainException e)
        {
            return null;
        }
    }

    public void RemovePointOfInterest(PointOfInterestId id)
    {
        var poi = _pointOfInterests.Find(x => x.Id == id);
        if (poi != null) _pointOfInterests.Remove(poi);
    }

    public void UpdatePointOfInterest(PointOfInterestId id, string name, string description, string poiType,
        double duration)
    {
        var poi = _pointOfInterests.Find(x => x.Id == id);
        if (poi != null)
            try
            {
                poi.Name = name;
                poi.Description = description;
                poi.PoiType = PoiType.From(poiType);
                poi.Duration = new TimeSchedule(duration);
                Console.WriteLine("Update POI Success!");
            }
            catch (DomainException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
    }

    public void CreateDestination(string name, string country, bool isOverSea)
    {
    }
}