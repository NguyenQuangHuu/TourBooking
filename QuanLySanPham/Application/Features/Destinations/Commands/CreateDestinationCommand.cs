using MediatR;
using QuanLySanPham.Application.Exceptions;
using QuanLySanPham.Domain.Aggregates.Destinations;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Application.Features.Destinations.Commands;

public record CreateDestinationCommand(string Name, string Country, bool IsOverSea) : IRequest<DestinationId?>;

public class CreateDestinationCommandHandler : IRequestHandler<CreateDestinationCommand, DestinationId?>
{
    private readonly IDestinationRepository _destinationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDestinationCommandHandler(IDestinationRepository destinationRepository, IUnitOfWork unitOfWork)
    {
        _destinationRepository = destinationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<DestinationId?> Handle(CreateDestinationCommand request, CancellationToken ct)
    {
        await _unitOfWork.BeginTransactionAsync(ct);
        var destination = await _destinationRepository.GetDestinationByNameAsync(request.Name, ct);
        if (destination is not null) throw new ExistException("Destination with that name already exists");
        var newDestination = new Destination(request.Name, request.Country, request.IsOverSea);
        var result = await _destinationRepository.CreateDestinationAsync(newDestination, ct);
        await _unitOfWork.CommitAsync(ct);
        return result;
    }
}