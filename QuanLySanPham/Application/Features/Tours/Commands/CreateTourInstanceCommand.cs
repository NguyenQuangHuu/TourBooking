using MediatR;
using QuanLySanPham.Domain.Aggregates.Tours;
using QuanLySanPham.Domain.Events.Tours;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Application.Features.Tours.Commands;

public class CreateTourInstanceCommand : IRequest<TourInstanceId?>
{
    public DateRange OperationalPeriod { get; set; }
    public SlotInfo SlotInfo { get; set; }
    public Money PricePerPax { get; set; }
    public TourMasterId TourMasterId { get; set; }

    public CreateTourInstanceCommand(DateRange operationalPeriod, SlotInfo slotInfo, Money pricePerPax,
        TourMasterId tourMasterId)
    {
        OperationalPeriod = operationalPeriod;
        SlotInfo = slotInfo;
        PricePerPax = pricePerPax;
        TourMasterId = tourMasterId;
    }
}

public class CreateTourInstanceCommandHandler(ITourManagementRepository repo, IUnitOfWork uow)
    : IRequestHandler<CreateTourInstanceCommand, TourInstanceId?>
{
    public async Task<TourInstanceId?> Handle(CreateTourInstanceCommand request, CancellationToken cancellationToken)
    {
        await uow.BeginTransactionAsync(cancellationToken);
        var tourInstance = new TourInstance(request.OperationalPeriod, request.SlotInfo, request.PricePerPax,
            request.TourMasterId);
        tourInstance.AddDomainEvent(new TourInstanceCreatedEvent(tourInstance.Id));
        var result = await repo.CreateTourInstance(tourInstance, cancellationToken);
        await uow.CommitAsync(cancellationToken);
        return result;
    }
}