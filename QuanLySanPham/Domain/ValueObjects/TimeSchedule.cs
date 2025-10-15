using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.Exceptions;

namespace QuanLySanPham.Domain.ValueObjects;

public class TimeSchedule : ValueObject
{
    public double Value { get; set; }

    public TimeSchedule(double value)
    {
        if (value <= 0) throw new DomainException("Value must be greater than zero");

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}