using QuanLySanPham.Domain.Aggregates.Invoices;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Presentations.DTOs.Responses;

namespace QuanLySanPham.Domain.Interfaces;

public interface IInvoiceRepository
{
    Task<Invoice> CreateInvoiceAsync(Invoice invoice,CancellationToken ct);
    Task<Invoice?> GetInvoiceByIdAsync(InvoiceId invoiceId, CancellationToken ct);
    Task<Invoice> UpdateInvoiceAsync(Invoice invoice, CancellationToken ct);
}