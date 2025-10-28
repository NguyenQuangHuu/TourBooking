namespace QuanLySanPham.Application.Services;

public interface IDomainEventDispatcher
{
    Task DispatchEventAsync(CancellationToken ct);
}