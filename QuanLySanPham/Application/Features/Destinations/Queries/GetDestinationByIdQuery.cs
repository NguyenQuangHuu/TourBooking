using MediatR;
using QuanLySanPham.Domain.Aggregates.Destinations;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Application.Features.Destinations.Queries;

public record GetDestinationByIdQuery(Guid Id) : IRequest<Destination?>;

public class GetDestinationByIdQueryHandler(IUnitOfWork unitOfWork, IDestinationRepository destinationRepository)
    : IRequestHandler<GetDestinationByIdQuery, Destination?>
{
    public async Task<Destination?> Handle(GetDestinationByIdQuery request, CancellationToken ct)
    {
        await unitOfWork.BeginAsync(ct);
        var destinationId = DestinationId.From(request.Id);
        return await destinationRepository.GetDestinationByIdAsync(destinationId, ct);
    }
}