using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.Exceptions;

namespace QuanLySanPham.Domain.ValueObjects;

public class SlotInfo : ValueObject
{
    public int OpenedSlot { get; set; }
    private int BookedSlot { get; set; }
    private int AvailableSlot { get; set; }

    public SlotInfo()
    {
    }

    public SlotInfo(int openedSlot)
    {
        if (openedSlot < 0) throw new DomainException("Opened Slot cannot be less than 0");
        OpenedSlot = openedSlot;
        BookedSlot = 0;
        AvailableSlot = openedSlot;
    }

    public SlotInfo(int openedSlot, int bookedSlot)
    {
        if (bookedSlot < 0) throw new DomainException("Booked Slot cannot be less than 0");
        if (bookedSlot > openedSlot) throw new DomainException("Booked Slot cannot be more than opened Slot");
        OpenedSlot = openedSlot;
        BookedSlot = bookedSlot;
        AvailableSlot = OpenedSlot - BookedSlot;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return OpenedSlot;
        yield return BookedSlot;
        yield return AvailableSlot;
    }
}