using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Aggregates.Invoices;

public class Invoice : BaseEntity<InvoiceId>
{
    public BookingId  BookingId { get; init; }
    public UserId UserId { get; init; }
    public Money TotalAmount { get; init; }
    public Money PaidAmount { get; set; }
    public InvoiceStatus InvoiceStatus { get; set; }

    public Invoice(BookingId  bookingId, UserId userId, Money totalAmount)
    {
        BookingId = bookingId;
        UserId = userId;
        TotalAmount = totalAmount;
        PaidAmount = new Money(0.0);
        InvoiceStatus = InvoiceStatus.Unpaid;
    }

    private void UpdateInvoiceStatus(InvoiceStatus invoiceStatus)
    {
        if (InvoiceStatus.CanChangeTo(invoiceStatus))
        {
            InvoiceStatus = invoiceStatus;
        }
        else
        {
            throw new DomainException("Trạng thái hóa đơn không hợp lệ!");
        }
    }

    public void InvoicePaid()
    {
        if (InvoiceStatus == InvoiceStatus.Unpaid)
        {
            PaidAmount = TotalAmount;
            UpdateInvoiceStatus(InvoiceStatus.Paid);
        }
        else
        {
            throw new DomainException("Trạng thái hóa đơn không hợp lệ");
        }
    }

    public void InvoiceCancel()
    {
        if (InvoiceStatus == InvoiceStatus.Unpaid)
        {
            UpdateInvoiceStatus(InvoiceStatus.Canceled);
        }
        else
        {
            throw new DomainException("Trạng thái hóa đơn không hợp lệ");
        }
    }

    public void InvoiceRefund()
    {
        if (InvoiceStatus == InvoiceStatus.Paid)
        {
            PaidAmount = new Money(0.0);
            UpdateInvoiceStatus(InvoiceStatus.Refunded);
        }
        else
        {
            throw new DomainException("Trạng thái hóa đơn không hợp lệ");
        }
    }
    
    
}