using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuanLySanPham.Application.Features.Commands.Payments;
using QuanLySanPham.Application.Features.Queries.Payments;
using QuanLySanPham.Domain.Aggregates.Payments;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Presentations.DTOs.Requests;
using QuanLySanPham.Presentations.DTOs.Responses;

namespace QuanLySanPham.Presentations.APIs;
[ApiController]
[Route("api/[controller]")]
public class PaymentsController: Controller
{
    private readonly IMediator _mediator;
    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<Result<Payment>> CreatePayment([FromBody] CreatePaymentRequest request, CancellationToken cancellationToken)
    {
        InvoiceId id = (InvoiceId)request.InvoiceId;
        Money money = new Money(request.TotalAmount);
        var cmd = new CreatePaymentCommand(id,money);
        var result =  await _mediator.Send(cmd,cancellationToken);
        return result;
    }

    [HttpGet("{paymentId}")]
    public async Task<Result<Payment>> GetPayment([FromRoute] Guid paymentId, CancellationToken cancellationToken)
    {
        PaymentId id = (PaymentId)paymentId;
        var command = new GetPaymentByIdQuery(id);
        var result = await _mediator.Send(command, cancellationToken);
        return result;
    }
}