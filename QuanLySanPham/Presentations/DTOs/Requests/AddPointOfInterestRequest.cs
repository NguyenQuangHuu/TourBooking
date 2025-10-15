namespace QuanLySanPham.Presentations.DTOs;

public record AddPointOfInterestRequest(string Name, string Description, string PointOfInterestType, double Duration);