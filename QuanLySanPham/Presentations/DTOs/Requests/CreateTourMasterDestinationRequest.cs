namespace QuanLySanPham.Presentations.DTOs.Requests;

public record CreateTourMasterDestinationRequest(Guid DestinationId, int Order, int StayDays);