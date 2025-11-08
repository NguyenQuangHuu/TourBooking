using MediatR;
using QuanLySanPham.Domain.Aggregates.Invoices;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Presentations.DTOs.Responses;

namespace QuanLySanPham.Application.Features.Queries.Invoices;

public record GetInvoiceByIdQuery(InvoiceId InvoiceId) : IRequest<Result<Invoice>>;

public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, Result<Invoice>>
{
    private readonly IDbContext  _dbContext;
    private readonly IInvoiceRepository _invoiceRepository;

    public GetInvoiceByIdQueryHandler(IDbContext dbContext, IInvoiceRepository invoiceRepository)
    {
        _dbContext = dbContext;
        _invoiceRepository = invoiceRepository;
    }
    public async Task<Result<Invoice>> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _invoiceRepository.GetInvoiceByIdAsync(request.InvoiceId,cancellationToken);
        if (result is null)
        {
            return Result<Invoice>.Failure($"Không tồn tại mã hóa đơn ${request.InvoiceId}",
                StatusCodes.Status400BadRequest);
        }
        else
        {
            return Result<Invoice>.Success(result, StatusCodes.Status200OK);
        }
    }
}