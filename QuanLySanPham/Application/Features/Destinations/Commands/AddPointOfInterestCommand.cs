using MediatR;
using QuanLySanPham.Application.Exceptions;
using QuanLySanPham.Domain.Aggregates.Destinations;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Application.Features.Destinations.Commands;

public record AddPointOfInterestCommand(
    Guid Id,
    string Name,
    double Duration,
    string PointOfInterestType,
    string Description) : IRequest<PointOfInterestId>
{
}

public class AddPointOfInterestCommandHandler : IRequestHandler<AddPointOfInterestCommand, PointOfInterestId?>
{
    private readonly IDestinationRepository _destinationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPointOfInterestCommandHandler(IDestinationRepository destinationRepository, IUnitOfWork unitOfWork)
    {
        _destinationRepository = destinationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PointOfInterestId?> Handle(AddPointOfInterestCommand request, CancellationToken ct)
    {
        await _unitOfWork.BeginTransactionAsync(ct);
        var destinationId = DestinationId.From(request.Id);
        var exist = await _destinationRepository.GetDestinationByIdAsync(destinationId, ct);
        if (exist is null) throw new NotFoundException("Destination not found");

        var poiType = PoiType.From(request.PointOfInterestType);
        var duration = new TimeSchedule(request.Duration);
        var poiAdded = new PointOfInterest(request.Name, request.Description, poiType,
            duration, destinationId);
        var result = await _destinationRepository.AddPoiAsync(poiAdded, ct);
        await _unitOfWork.CommitAsync(ct);
        return result;
    }
}