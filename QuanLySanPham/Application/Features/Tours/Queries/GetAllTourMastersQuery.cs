using MediatR;
using QuanLySanPham.Domain.Aggregates.Tours;
using QuanLySanPham.Domain.Interfaces;

namespace QuanLySanPham.Application.Features.Tours.Queries;

public record GetAllTourMastersQuery : IRequest<IReadOnlyList<TourMaster>>;

public class GetAllTourMasterHandler(ITourManagementRepository repo, IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllTourMastersQuery, IReadOnlyList<TourMaster>>
{
    public async Task<IReadOnlyList<TourMaster>> Handle(GetAllTourMastersQuery request,
        CancellationToken ct)
    {
        await unitOfWork.BeginTransactionAsync(ct);
        return await repo.GetAllTourMasters(ct);
    }
}