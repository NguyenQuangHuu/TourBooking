using MediatR;
using QuanLySanPham.Domain.Aggregates.Destinations;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Application.Features.Destinations.Queries;

public record GetPoiByDestinationIdQuery(DestinationId DestinationId) : IRequest<IReadOnlyList<PointOfInterest>?>;

public class GetPoiByDestinationIdQueryHandler(IDestinationRepository repo, IUnitOfWork uow)
    : IRequestHandler<GetPoiByDestinationIdQuery, IReadOnlyList<PointOfInterest>?>
{
    public async Task<IReadOnlyList<PointOfInterest>?> Handle(GetPoiByDestinationIdQuery request,
        CancellationToken ct)
    {
        await uow.BeginAsync(ct);
        return await repo.GetPointOfInterestByDestinationIdAsync(request.DestinationId, ct);
    }
}