using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects;

public class Quantity : ValueObject
{
    public int Value { get; set; }

    private Quantity()
    {
    }

    public Quantity(int value)
    {
        if (value < 0) throw new ArgumentException("Value must be greater than or equal to 0");
        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator int(Quantity quantity)
    {
        return quantity.Value;
    }
}