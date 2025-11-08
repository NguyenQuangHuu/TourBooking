namespace QuanLySanPham.Presentations.DTOs.Requests;

public record CreatePaymentRequest(Guid InvoiceId,double TotalAmount,string Currency);