using Npgsql;
using QuanLySanPham.Domain.Aggregates.Invoices;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Infrastructure.Persistence.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly IUnitOfWork  _unitOfWork;

    public InvoiceRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Invoice> CreateInvoiceAsync(Invoice invoice, CancellationToken ct)
    {
        string sql = "insert into invoices(booking_id,total,invoice_status)  values(@BookingId,@TotalAmount,@InvoiceStatus)";
        await using NpgsqlCommand cmd = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        cmd.Parameters.AddWithValue("@BookingId", invoice.BookingId.Value);
        cmd.Parameters.AddWithValue("@TotalAmount", invoice.TotalAmount.Amount);
        cmd.Parameters.AddWithValue("@InvoiceStatus", invoice.InvoiceStatus.Value);
        var result =  await cmd.ExecuteScalarAsync(ct);
        if (result is not null)
        {
            invoice.Id = (InvoiceId)result;
            return invoice;
        }
        else
        {
            throw new DomainException("Lỗi tạo hóa đơn");
        }
    }

    public async Task<Invoice?> GetInvoiceByIdAsync(InvoiceId invoiceId, CancellationToken ct)
    {
        string sql =  "select id, booking_id,total,invoice_status,created_at from invoices where id=@InvoiceId";
        await using NpgsqlCommand cmd = new NpgsqlCommand(sql, _unitOfWork.Connection);
        cmd.Parameters.AddWithValue("@InvoiceId", invoiceId.Value);
        await using  NpgsqlDataReader reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return new Invoice
            {
                Id = (InvoiceId)reader.GetGuid(0),
                BookingId = BookingId.From(reader.GetGuid(1)),
                TotalAmount = new Money(reader.GetDouble(2)),
                InvoiceStatus = InvoiceStatus.From(reader.GetString(3)),
                CreatedOn = reader.IsDBNull(4) ? default : reader.GetDateTime(4)
            };
        }
        return null;
    }

    public async Task<Invoice> UpdateInvoiceAsync(Invoice invoice, CancellationToken ct)
    {
        string sql =
            "update invoices set total = @Amount, invoice_status = @InvoiceStatus, modified_at = @ModifiedAt where id = @InvoiceId";
        await using  NpgsqlCommand cmd = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        cmd.Parameters.AddWithValue("@Amount", invoice.TotalAmount.Amount);
        cmd.Parameters.AddWithValue("@InvoiceStatus", invoice.InvoiceStatus.Value);
        cmd.Parameters.AddWithValue("@ModifiedAt", DateTime.UtcNow);
        cmd.Parameters.AddWithValue("@InvoiceId", invoice.Id.Value);
        var result =   await cmd.ExecuteNonQueryAsync(ct);
        if (result <= 0)
        {
            throw new DomainException("cập nhật hóa đơn không thành công!");
        }
        return invoice;
    }
}