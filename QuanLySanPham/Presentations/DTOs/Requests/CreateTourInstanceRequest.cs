namespace QuanLySanPham.Presentations.DTOs.Requests;

public record CreateTourInstanceRequest(DateOnly StartDate, DateOnly EndDate, double PricePerTax, int OpenedSlots)
{
}