using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuanLySanPham.Application.Features.Destinations.Commands;
using QuanLySanPham.Application.Features.Destinations.Queries;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Presentations.DTOs;

namespace QuanLySanPham.Presentations.APIs;

[ApiController]
[Route("api/[controller]")]
public class DestinationsController : Controller
{
    private readonly IMediator _mediator;

    public DestinationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetDestinations(CancellationToken token = default)
    {
        var getDestinations = new GetAllDestinationsQuery();
        var result = await _mediator.Send(getDestinations, token);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDestination([FromBody] CreateDestinationRequest request)
    {
        var command = new CreateDestinationCommand(request.Name, request.Country, request.IsOverSea);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // GET
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDestinationById(Guid id)
    {
        var query = new GetDestinationByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}/points-of-interest")]
    public async Task<IActionResult> GetPointOfInterestByDestinationId(Guid id)
    {
        var destinationId = DestinationId.From(id);
        var query = new GetPoiByDestinationIdQuery(destinationId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("{id}/points-of-interest")]
    public async Task<IActionResult> AddPointOfInterest([FromRoute] Guid id, [FromBody] AddPointOfInterestRequest req)
    {
        var poiCommand =
            new AddPointOfInterestCommand(id, req.Name, req.Duration, req.PointOfInterestType, req.Description);
        var result = await _mediator.Send(poiCommand);
        return Ok(result);
    }
}