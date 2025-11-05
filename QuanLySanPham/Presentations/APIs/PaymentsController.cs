using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuanLySanPham.Application.Features.Commands.Payments;
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
    public async Task<Result<string>> Payment([FromRoute] Guid invoiceId,[FromBody] CreatePaymentRequest request, CancellationToken cancellationToken)
    {
        var cmd = new CreatePaymentCommand((InvoiceId)invoiceId,request.TotalAmount, request.Currency, request.Message);
        var result =  await _mediator.Send(request,cancellationToken);
        return Result<string>.Success("Tạo hóa đơn thành công",StatusCodes.Status201Created);
    }
    
}