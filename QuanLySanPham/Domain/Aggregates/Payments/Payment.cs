using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Aggregates.Payments;

public class Payment : BaseEntity<PaymentId>
{
    public Money Amount { get; init; }
    public PaymentStatus PaymentStatus { get; set; }
    public InvoiceId InvoiceId { get; init; }

    public Payment()
    {
    }

    public Payment(InvoiceId invoiceId, Money amount)
    {
        InvoiceId = invoiceId;
        Amount = amount;
        PaymentStatus = PaymentStatus.Pending;
    }

    public void MarkAsFailed()
    {
        PaymentStatus = PaymentStatus.ChangePaymentStatus(PaymentStatus.Failed);
        ModifiedAt = DateTime.Now;
    }

    public void MarkAsSucceeded()
    {
        PaymentStatus = PaymentStatus.ChangePaymentStatus(PaymentStatus.Completed);
        ModifiedAt = DateTime.Now;
    }

    public void MarkAsExpired()
    {
        PaymentStatus = PaymentStatus.ChangePaymentStatus(PaymentStatus.Expired);
        ModifiedAt = DateTime.Now;
    }
}