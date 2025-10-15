using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects.Ids;

public class BookingId : EntityId
{
    public BookingId(Guid value) : base(value)
    {
    }

    public static BookingId From(Guid id)
    {
        return new BookingId(id);
    }
}