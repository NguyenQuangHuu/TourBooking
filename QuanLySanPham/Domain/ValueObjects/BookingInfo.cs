using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.ValueObjects;

public class BookingInfo : ValueObject
{
    public int BookedSlot { get; set; }
    public UserId UserId { get; set; }
    public int AdultQuantity { get; set; }
    public int ChildQuantity { get; set; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}