using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects.Auth;

public class Username : ValueObject
{
    public string Value { get; set; }

    public Username(string value)
    {
        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}