using Npgsql;
using QuanLySanPham.Domain.Aggregates.Payments;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Infrastructure.Exceptions;

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
        string sql = "insert into payments(invoice_id , amount) values(@InvoiceId,@Amount) returning id";
        await using NpgsqlCommand cmd = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        cmd.Parameters.Add(new NpgsqlParameter("@InvoiceId", payment.InvoiceId.Value));
        cmd.Parameters.Add(new NpgsqlParameter("@Amount", payment.Amount));
        var result = await cmd.ExecuteScalarAsync(ct);
        if (result is null)
        {
            throw new InfrastructureException("Có lỗi khi tạo payment");
        }
        else
        {
            PaymentId invoiceId = (PaymentId)result;
            payment.Id = invoiceId;
            return  payment;
        }
    }

    public async Task<Payment?> GetAsync(PaymentId id, CancellationToken ct)
    {
        string sql = "select id,created_at,amount,invoice_id,payment_status from payments where id=@id";
        await using NpgsqlCommand cmd = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        cmd.Parameters.Add(new NpgsqlParameter("@id", id.Value));
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (await reader.ReadAsync(ct))
        {
            return new Payment
            {
                Id = (PaymentId)reader.GetGuid(0),
                Amount = new Money(reader.GetDouble(2)),
                CreatedAt = reader.GetDateTime(1),
                InvoiceId = (InvoiceId)reader.GetGuid(3),
                PaymentStatus = PaymentStatus.From(reader.GetString(4)),
            };
        }

        return null;
    }

    public async Task<IReadOnlyList<Payment>> GetPaymentsByInvoiceIdAsync(InvoiceId invoiceId, CancellationToken ct)
    {
        string sql =
            "select id,amount,created_at,modified_at,payment_status from payments where invoice_id = @InvoiceId";
        await using NpgsqlCommand cmd = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        cmd.Parameters.Add(new NpgsqlParameter("@InvoiceId", invoiceId.Value));
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        var payments = new List<Payment>();
        while (await reader.ReadAsync(ct))
        {
            Payment payment = new Payment
            {
                Id = (PaymentId)reader.GetGuid(0),
                Amount = new Money(reader.GetDouble(1)),
                CreatedAt = reader.GetDateTime(2),
                ModifiedAt = reader.GetDateTime(3),
                PaymentStatus = PaymentStatus.From(reader.GetString(4)),
                InvoiceId = (InvoiceId)invoiceId.Value,
            };
            payments.Add(payment);
        }
        return payments;
    }

    public async Task<Payment> UpdateAsync(Payment payment, CancellationToken ct)
    {
        string sql = "update payments set payment_status = @NewPaymentStatus, modified_at = @LastUpdated where id = @PaymentId";
        await using  NpgsqlCommand cmd = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        cmd.Parameters.Add(new NpgsqlParameter("@PaymentId", payment.Id));
        cmd.Parameters.Add(new NpgsqlParameter("@NewPaymentStatus", payment.PaymentStatus));
        cmd.Parameters.Add(new NpgsqlParameter("@LastUpdated", DateTime.Now));
        var result = await cmd.ExecuteNonQueryAsync(ct); // trả về số record bị ảnh hưởng
        if (result > 0)// nếu bé hơn hoặc = 0 là thao tác bị lỗi hoặc không có record nào bị ảnh hưởng
        {
            return payment;
        }
        else
        {
            throw new InfrastructureException("Lỗi thao tác cập nhật với dữ liệu");
        }
    }
}