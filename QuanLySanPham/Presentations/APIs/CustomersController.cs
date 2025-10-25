using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuanLySanPham.Application.Exceptions;
using QuanLySanPham.Application.Features.Customers.Commands;
using QuanLySanPham.Application.Features.Customers.Queries;
using QuanLySanPham.Domain.Aggregates.Customers;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Presentations.DTOs.Requests;
using QuanLySanPham.Presentations.DTOs.Responses;

namespace QuanLySanPham.Presentations.APIs;
[ApiController]
[Route("api/[controller]")]
public class CustomersController : Controller
{
    private readonly IMediator  _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerInformation([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            var queryById = new GetCustomerById(id);
            var result = await _mediator.Send(queryById, ct);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }

    }

    [HttpPost("{id}")]
    public async Task<IActionResult> UpdateCustomerInformation([FromRoute] Guid id,
        [FromBody] CustomerUpdateInformationRequest request, CancellationToken ct)
    {
        try
        {
            UserId userId = UserId.From(id);
            var command = new CustomerUpdateInformationCommand(request.DisplayName, request.DateOfBirth, request.Gender,
                request.IdentityCardNumber, request.Address, userId);
            var result = await _mediator.Send(command, ct);
            return Ok(result);
        }
        catch (DomainException ex)
        {
            return BadRequest(Result<string>.Failure(ex.Message,StatusCodes.Status400BadRequest));
        }


    }
}