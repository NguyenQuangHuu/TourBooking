using MediatR;
using QuanLySanPham.Application.Exceptions;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Application.Features.Commands;

public record PayInvoiceCommand(InvoiceId InvoiceId): IRequest;

public class PayInvoiceCommandHandler : IRequestHandler<PayInvoiceCommand>
{
    private readonly IInvoiceRepository _invoiceRepository;

    public PayInvoiceCommandHandler(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }
    public async Task Handle(PayInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetInvoiceByIdAsync(request.InvoiceId, cancellationToken);
        if (invoice is null)
        {
            throw new NotFoundException("Không tồn tại hóa đơn này!");
        }

        if (invoice.InvoiceStatus != InvoiceStatus.Unpaid)
        {
            throw new DomainException("Trạng thái hóa đơn không hợp lệ!");
        }
        
    }
}