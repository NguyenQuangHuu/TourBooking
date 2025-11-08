using MediatR;
using QuanLySanPham.Domain.Aggregates.Payments;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Presentations.DTOs.Responses;

namespace QuanLySanPham.Application.Features.Queries.Payments;

public record GetPaymentByIdQuery(PaymentId PaymentId): IRequest<Result<Payment>>
{
    
}

public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery,Result<Payment>>
{
    private readonly IUnitOfWork  _unitOfWork;
    private readonly IPaymentRepository  _paymentRepository;

    public GetPaymentByIdQueryHandler(IUnitOfWork unitOfWork, IPaymentRepository paymentRepository)
    {
        _unitOfWork = unitOfWork;
        _paymentRepository = paymentRepository;
    }
    public async Task<Result<Payment>> Handle(GetPaymentByIdQuery query, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginAsync(cancellationToken);
        var result = await _paymentRepository.GetAsync(query.PaymentId, cancellationToken);
        if (result is null)
        {
            return Result<Payment>.Failure($"Không tìm mã thanh toán {query.PaymentId}", StatusCodes.Status404NotFound);
        }
        else
        {
            return Result<Payment>.Success(result, StatusCodes.Status200OK);
        }
    }
}