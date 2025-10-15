using QuanLySanPham.Domain.Aggregates.Destinations;
using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Domain.Aggregates.Tours;

public class TourMaster : BaseEntity<TourMasterId>
{
    public string Name { get; set; }
    public Money OperatingCost { get; set; }
    public TimeSchedule DurationEstimate { get; set; }

    private readonly List<TourMasterDestination> _destinations = new();

    public IReadOnlyList<TourMasterDestination> Destinations => _destinations;

    private readonly List<TourMasterPoi> _pointOfInterest = new();

    public IReadOnlyList<TourMasterPoi> PointOfInterest => _pointOfInterest;

    public TourMaster()
    {
    }

    public TourMaster(string name, Money operatingCost, TimeSchedule durationEstimate)
    {
        Name = name;
        OperatingCost = operatingCost;
        DurationEstimate = durationEstimate;
    }

    public TourMaster(string name, Money operatingCost, TimeSchedule durationEstimate,
        List<TourMasterDestination> destinations, List<TourMasterPoi> pointOfInterest)
    {
        Name = name;
        OperatingCost = operatingCost;
        DurationEstimate = durationEstimate;
        _destinations.AddRange(destinations);
        _pointOfInterest.AddRange(pointOfInterest);
    }

    public void AddTourMasterDestination(TourMasterDestination destination)
    {
        if (!_destinations.Contains(destination))
            _destinations.Add(destination);
        throw new DomainException("Already exists");
    }

    public void AddTourMasterPoi(TourMasterPoi poi)
    {
        if (!_pointOfInterest.Contains(poi))
            _pointOfInterest.Add(poi);
        throw new DomainException("Already exists");
    }
}