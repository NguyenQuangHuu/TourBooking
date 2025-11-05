using MediatR;
using QuanLySanPham.Domain.Aggregates.Payments;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Presentations.DTOs.Responses;

namespace QuanLySanPham.Application.Features.Commands.Payments;

public record CreatePaymentCommand(InvoiceId InvoiceId,double Amount,string Currency,string Message): IRequest<Result<Payment>>;


public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<Payment>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IInvoiceRepository  _invoiceRepository;
    private readonly IPaymentRepository _paymentRepository;

    public CreatePaymentCommandHandler(IUnitOfWork  unitOfWork, IInvoiceRepository invoiceRepository, IPaymentRepository paymentRepository)
    {
        _unitOfWork = unitOfWork;
        _invoiceRepository = invoiceRepository;
        _paymentRepository = paymentRepository;
    }

    public async Task<Result<Payment>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
