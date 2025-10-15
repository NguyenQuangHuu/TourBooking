using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;

namespace QuanLySanPham.Domain.Aggregates.Tours;

public class TourInstance : BaseEntity<TourInstanceId>
{
    public DateRange OperationalPeriod { get; set; }
    public SlotInfo SlotInfo { get; set; }
    public Money PricePerPax { get; set; }
    public TourMasterId TourMasterId { get; set; }

    public TourInstance()
    {
    }

    public TourInstance(DateRange operationalPeriod, SlotInfo slotInfo, Money pricePerPax, TourMasterId tourMasterId)
    {
        OperationalPeriod = operationalPeriod;
        PricePerPax = pricePerPax;
        SlotInfo = slotInfo;
        TourMasterId = tourMasterId;
    }
}