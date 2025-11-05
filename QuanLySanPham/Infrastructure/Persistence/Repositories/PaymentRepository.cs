using QuanLySanPham.Domain.Aggregates.Payments;
using QuanLySanPham.Domain.Interfaces;

namespace QuanLySanPham.Infrastructure.Persistence.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public PaymentRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Payment> AddAsync(Payment payment, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}