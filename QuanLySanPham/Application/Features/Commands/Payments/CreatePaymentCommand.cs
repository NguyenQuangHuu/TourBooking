using MediatR;
using QuanLySanPham.Application.Services;
using QuanLySanPham.Domain.Aggregates.Payments;
using QuanLySanPham.Domain.Events.Payments;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Presentations.DTOs.Responses;

namespace QuanLySanPham.Application.Features.Commands.Payments;

public record CreatePaymentCommand(InvoiceId InvoiceId,Money Amount): IRequest<Result<Payment>>;


public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<Payment>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IInvoiceRepository  _invoiceRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ITrackedEntities _trackedEntities;

    public CreatePaymentCommandHandler(IUnitOfWork  unitOfWork, IInvoiceRepository invoiceRepository, IPaymentRepository paymentRepository,  ITrackedEntities trackedEntities)
    {
        _unitOfWork = unitOfWork;
        _invoiceRepository = invoiceRepository;
        _paymentRepository = paymentRepository;
        _trackedEntities = trackedEntities;
    }

    public async Task<Result<Payment>> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        var invoiceId = await _invoiceRepository.GetInvoiceByIdAsync(command.InvoiceId, cancellationToken);
        if (invoiceId is null)
        {
            return Result<Payment>.Failure($"Không tìm thấy hóa đơn {command.InvoiceId}",
                StatusCodes.Status400BadRequest);
        }

        Payment payment = new Payment(command.InvoiceId, command.Amount);
        var result = await _paymentRepository.AddAsync(payment, cancellationToken);
        result.AddDomainEvent(new PaymentCreatedEvent(result.Id,result.InvoiceId));
        _trackedEntities.TrackEntity(result);
        await _unitOfWork.CommitAsync(cancellationToken);
        return Result<Payment>.Success(result, StatusCodes.Status201Created);
    }
}
