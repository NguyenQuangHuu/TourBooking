using MediatR;
using QuanLySanPham.Application.Exceptions;
using QuanLySanPham.Domain.Aggregates.Destinations;
using QuanLySanPham.Domain.Aggregates.Tours;
using QuanLySanPham.Domain.Events.Tours;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Application.Features.Tours.Commands;

public class CreateTourMasterCommand : IRequest<TourMasterId?>
{
    public string TourName { get; set; }
    public TimeSchedule DurationEstimate { get; set; }
    public Money OperatingCost { get; set; }

    private readonly List<TourMasterDestination> _destinations = new();

    public IReadOnlyList<TourMasterDestination> Destinations => _destinations.AsReadOnly();

    private readonly List<TourMasterPoi> _pois = new();

    public IReadOnlyList<TourMasterPoi> Pois => _pois.AsReadOnly();

    public CreateTourMasterCommand(string tourName, Money operatingCost, TimeSchedule durationEstimate,
        List<TourMasterDestination> destinationIds, List<TourMasterPoi> poiIds)
    {
        TourName = tourName;
        OperatingCost = operatingCost;
        DurationEstimate = durationEstimate;
        _destinations.AddRange(destinationIds);
        _pois.AddRange(poiIds);
    }
}

public class CreateTourMasterCommandHandler(
    ITourManagementRepository tourManagementRepository,
    IDestinationRepository destinationRepo,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateTourMasterCommand, TourMasterId?>
{
    public async Task<TourMasterId?> Handle(CreateTourMasterCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        foreach (var value in request.Destinations)
        {
            var exists = await destinationRepo.GetDestinationByIdAsync(value.DestinationId, cancellationToken);
            if (exists is null) throw new NotFoundException("Destination not found");
        }

        foreach (var value in request.Pois)
        {
            var result = await destinationRepo.GetPointOfInterestByIdAsync(value.PointOfInterestId, cancellationToken);
            if (result is null)
                throw new NotFoundException($"Not Found Point Of Interest with Id : {value.PointOfInterestId}");
        }

        try
        {
            var tourMaster = new TourMaster(request.TourName, request.OperatingCost, request.DurationEstimate,
                request.Destinations.ToList(), request.Pois.ToList());
            var result = await tourManagementRepository.CreateTourMaster(tourMaster, cancellationToken);

            if (result is not null)
            {
                var id = result;
                await tourManagementRepository.InsertTourMasterDestinations(id, tourMaster.Destinations.ToList(),
                    cancellationToken);
                await tourManagementRepository.InsertTourMasterPoi(id, tourMaster.PointOfInterest.ToList(),
                    cancellationToken);
                tourMaster.AddDomainEvent(new TourMasterCreatedEvent(tourMaster.Id, tourMaster.Destinations.ToList(),
                    tourMaster.PointOfInterest.ToList()));
                return id;
            }

            await unitOfWork.CommitAsync(cancellationToken);
            return result;
        }
        catch (DomainException ex)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}