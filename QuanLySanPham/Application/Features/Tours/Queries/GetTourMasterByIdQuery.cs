using MediatR;
using QuanLySanPham.Domain.Aggregates.Tours;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Application.Features.Tours.Queries;

public record GetTourMasterByIdQuery(Guid Id) : IRequest<TourMaster?>;

public class GetTourMasterByIdHandler(ITourManagementRepository repo, IUnitOfWork uow)
    : IRequestHandler<GetTourMasterByIdQuery, TourMaster?>
{
    public async Task<TourMaster?> Handle(GetTourMasterByIdQuery request, CancellationToken cancellationToken)
    {
        await uow.BeginTransactionAsync(cancellationToken);
        return await repo.GetTourMasterById(TourMasterId.From(request.Id), cancellationToken);
    }
}