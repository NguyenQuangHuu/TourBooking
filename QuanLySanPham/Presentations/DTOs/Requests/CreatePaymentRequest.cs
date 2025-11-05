namespace QuanLySanPham.Presentations.DTOs.Requests;

public record CreatePaymentRequest(double TotalAmount,string Currency,string FromAccount ,string ToAccount, string Message);