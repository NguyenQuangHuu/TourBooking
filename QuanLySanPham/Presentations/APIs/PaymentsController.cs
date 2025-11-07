using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuanLySanPham.Application.Features.Commands.Payments;
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
    [HttpPost("{invoiceId}/create-payment")]
    public async Task<Result<Payment>> Payment([FromRoute] Guid invoiceId,[FromBody] CreatePaymentRequest request, CancellationToken cancellationToken)
    {
        InvoiceId id = (InvoiceId)invoiceId;
        Money money = new Money(request.TotalAmount);
        var cmd = new CreatePaymentCommand(id,money);
        var result =  await _mediator.Send(cmd,cancellationToken);
        return result;
    }
    
    
}