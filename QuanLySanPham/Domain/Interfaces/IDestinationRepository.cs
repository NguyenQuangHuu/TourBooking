using QuanLySanPham.Domain.Aggregates.Destinations;
using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Domain.Interfaces;

public interface IDestinationRepository
{
    Task<DestinationId?> CreateDestinationAsync(Destination destination, CancellationToken token);

    Task<Destination?> GetDestinationByNameAsync(string name, CancellationToken token);
    Task<Destination?> GetDestinationByIdAsync(DestinationId id, CancellationToken token);
    Task<PointOfInterestId?> AddPoiAsync(PointOfInterest poi, CancellationToken token);
    Task<IReadOnlyList<Destination>?> GetAllDestinationsAsync(CancellationToken token);
    Task<Destination?> GetDestinationWithPoiByIdAsync(DestinationId id, CancellationToken token);
    Task<PointOfInterest?> GetPointOfInterestByIdAsync(PointOfInterestId id, CancellationToken token);

    Task<IReadOnlyList<PointOfInterest>?> GetPointOfInterestByDestinationIdAsync(DestinationId destinationId,
        CancellationToken token);
}