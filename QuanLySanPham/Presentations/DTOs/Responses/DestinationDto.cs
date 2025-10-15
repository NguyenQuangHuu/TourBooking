using QuanLySanPham.Domain.Aggregates.Destinations;

namespace QuanLySanPham.Presentations.DTOs;

public class DestinationDto
{
    public string Name { get; set; }
    public string Country { get; set; }
    public bool IsOverSea { get; set; }

    public DestinationDto(Destination destination)
    {
        Name = destination.Name;
        Country = destination.Country;
        IsOverSea = destination.IsOverSea;
    }
}