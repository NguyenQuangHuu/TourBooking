using QuanLySanPham.Domain.Aggregates.Payments;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> AddAsync(Payment payment, CancellationToken ct);
    Task<Payment?> GetAsync(PaymentId id, CancellationToken ct);
    
    Task<IReadOnlyList<Payment>> GetPaymentsByInvoiceIdAsync(InvoiceId invoiceId,CancellationToken ct);
    Task<Payment> UpdateAsync(Payment payment, CancellationToken ct);
}