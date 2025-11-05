using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Aggregates.Payments;

public class Payment : BaseEntity<PaymentId>
{
    public double Amount { get; init; }
    public string Currency { get; init; }
    public string Message { get; set; }
    public string FromAccount { get; init; }
    public string ToAccount { get; init; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PaymentAt { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public InvoiceId InvoiceId { get; init; }
    
}