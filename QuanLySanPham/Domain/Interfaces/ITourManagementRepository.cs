using QuanLySanPham.Domain.Aggregates.Tours;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Domain.Interfaces;

public interface ITourManagementRepository
{
    Task<TourMasterId?> CreateTourMaster(TourMaster tourMaster, CancellationToken token);
    Task<TourMaster?> GetTourMasterByTourName(string tourName, CancellationToken token);
    Task<IReadOnlyList<TourMaster>> GetAllTourMasters(CancellationToken token);
    Task<TourMaster?> GetTourMasterById(TourMasterId id, CancellationToken token);

    Task InsertTourMasterDestinations(TourMasterId tourMasterId, List<TourMasterDestination> list,
        CancellationToken token);

    Task InsertTourMasterPoi(TourMasterId tourMasterId, List<TourMasterPoi> list, CancellationToken token);
    Task<TourInstanceId?> CreateTourInstance(TourInstance tourInstance, CancellationToken token);

    Task<IReadOnlyList<TourInstance>> GetTourInstancesByTourMasterIdAsync(TourMasterId tourMasterId,
        CancellationToken token);
    
    Task<TourInstance?> GetTourInstanceByIdAsync(TourInstanceId tourInstanceId, CancellationToken token);
    
    Task<int> UpdateTourInstanceAsync(TourInstance tourInstance, CancellationToken token);
}