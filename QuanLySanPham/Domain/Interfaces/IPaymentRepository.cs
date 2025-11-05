using QuanLySanPham.Domain.Aggregates.Payments;

namespace QuanLySanPham.Domain.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> AddAsync(Payment payment, CancellationToken ct);
}