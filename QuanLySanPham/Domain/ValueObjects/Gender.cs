using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects;

public class Gender : ValueObject
{
    public static Gender Male => new("Male");
    public static Gender Female => new("Female");
    public static Gender Undefined => new("Undefined");
    private string Value { get; set; }

    public Gender(string gender)
    {
        Value = gender;
    }

    public static implicit operator string(Gender gender)
    {
        return gender.Value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}