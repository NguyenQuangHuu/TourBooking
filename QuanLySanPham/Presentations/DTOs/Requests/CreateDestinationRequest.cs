namespace QuanLySanPham.Presentations.DTOs;

public record CreateDestinationRequest(string Name, string Country, bool IsOverSea)
{
}