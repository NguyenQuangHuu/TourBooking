using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Application.Services;

public interface ITrackedEntities
{
    void TrackEntity(IEntity entity);
    IReadOnlyList<IEntity> GetTrackedEntities { get; }
}