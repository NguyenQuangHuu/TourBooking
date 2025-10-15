namespace QuanLySanPham.Presentations.DTOs.Requests;

public record CreateTourMasterRequest(
    string Name,
    double OperatingCost,
    double DurationEstimate,
    List<CreateTourMasterDestinationRequest> Destinations,
    List<CreateTourMasterPoiRequest> PointOfInterests);