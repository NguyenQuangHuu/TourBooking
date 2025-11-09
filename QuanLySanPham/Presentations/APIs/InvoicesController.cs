using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuanLySanPham.Application.Features.Commands;
using QuanLySanPham.Application.Features.Queries.Invoices;
using QuanLySanPham.Domain.Aggregates.Invoices;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Presentations.DTOs.Responses;

namespace QuanLySanPham.Presentations.APIs;
[ApiController]
[Route("api/{controller}")]
public class InvoicesController : Controller
{
    private readonly IMediator _mediator;

    public InvoicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{invoiceId}")]
    public async Task<Result<Invoice>> GetInvoice(Guid invoiceId, CancellationToken ct)
    {
        InvoiceId id = (InvoiceId)invoiceId;
        var query = new GetInvoiceByIdQuery(id);
        return await _mediator.Send(query,ct);
    }

    [HttpGet("{invoiceId}/payments")]
    public async Task<IActionResult> PayInvoice(Guid invoiceId, CancellationToken ct)
    {
        var cmd = new PayInvoiceCommand((InvoiceId)invoiceId);
        await _mediator.Send(cmd, ct);
        return Ok();
    }
}