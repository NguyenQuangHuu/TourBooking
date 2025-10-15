using MediatR;
using QuanLySanPham.Domain.Aggregates.Tours;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Application.Features.Tours.Queries;

public class GetTourInstanceByTourMasterIdQuery : IRequest<IReadOnlyList<TourInstance>>
{
    public TourMasterId TourMasterId { get; }

    public GetTourInstanceByTourMasterIdQuery(TourMasterId tourMasterId)
    {
        TourMasterId = tourMasterId;
    }
}

public class
    GeTourInstanceByTourMasterIdQueryHandler : IRequestHandler<GetTourInstanceByTourMasterIdQuery,
    IReadOnlyList<TourInstance>>
{
    private readonly ITourManagementRepository _repository;
    private readonly IUnitOfWork _uow;

    public GeTourInstanceByTourMasterIdQueryHandler(ITourManagementRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<IReadOnlyList<TourInstance>> Handle(GetTourInstanceByTourMasterIdQuery query,
        CancellationToken cancellationToken)
    {
        await _uow.BeginAsync(cancellationToken);
        return await _repository.GetTourInstancesByTourMasterIdAsync(query.TourMasterId, cancellationToken);
    }
}