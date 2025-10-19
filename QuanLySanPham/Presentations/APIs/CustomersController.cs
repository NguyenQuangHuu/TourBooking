using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuanLySanPham.Application.Features.Customers.Commands;
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
        await Task.Delay(2000, ct);
        return Ok();
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> UpdateCustomerInformation([FromRoute] Guid id,
        [FromBody] CustomerUpdateInformationRequest request, CancellationToken ct)
    {
        try
        {
            UserId userId = UserId.From(id);
            Customer customer = new Customer(request.DisplayName, request.DateOfBirth, request.Gender,
                request.IdentityCardNumber, request.Address, userId);
            var command = new CustomerUpdateInformationCommand(customer);
            var result = await _mediator.Send(command, ct);
            return Ok(result);
        }
        catch (DomainException ex)
        {
            return BadRequest(ApiResponse<string>.Failure(ex.Message));
        }


    }
}