using MediatR;
using QuanLySanPham.Domain.Aggregates.Destinations;
using QuanLySanPham.Domain.Interfaces;

namespace QuanLySanPham.Application.Features.Destinations.Queries;

public record GetAllDestinationsQuery : IRequest<IReadOnlyList<Destination>?>;

public class GetAllDestinationQueryHandler(IUnitOfWork unitOfWork, IDestinationRepository destinationRepository)
    : IRequestHandler<GetAllDestinationsQuery, IReadOnlyList<Destination>?>
{
    public async Task<IReadOnlyList<Destination>?> Handle(GetAllDestinationsQuery request,
        CancellationToken ct)
    {
        await unitOfWork.BeginAsync(ct);
        return await destinationRepository.GetAllDestinationsAsync(ct);
    }
}