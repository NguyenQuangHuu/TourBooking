using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects;

public class Gender : ValueObject
{
    private IReadOnlyList<Gender> _genders = new List<Gender>(){Male, Female,Undefined};
    public static Gender Male => new("Male");
    public static Gender Female => new("Female");
    public static Gender Undefined => new("Undefined");
    private string Value { get; set; }

    private Gender(string gender)
    {
        if(!_genders.Contains((Gender)gender))
            throw new ArgumentException("Invalid gender");
        Value = gender;
    }

    public static implicit operator string(Gender gender)
    {
        return gender.Value;
    }
    
    public static explicit operator Gender(string gender)=> new (gender);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}