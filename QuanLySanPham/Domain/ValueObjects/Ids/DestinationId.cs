using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects;

public class DestinationId : EntityId
{
    private Guid Value { get; set; }

    public DestinationId(Guid value) : base(value)
    {
    }

    public static implicit operator Guid(DestinationId destinationId)
    {
        return destinationId.Value;
    }

    public static DestinationId CreateId => new(Guid.NewGuid());

    public static DestinationId From(Guid id)
    {
        return new DestinationId(id);
    }
}