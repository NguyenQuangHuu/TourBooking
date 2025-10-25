using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects;

public class PassengerInfo : ValueObject
{
    public string FullName  { get; set; }
    public DateOnly DateOfBirth  { get; set; }
    public Gender Gender { get; set; }
    public AgeGroup  AgeGroup { get; set; }
    public string IdentityCardNo { get; set; }
    public string PassportNo { get; set; }

    public PassengerInfo(string fullName, DateOnly dateOfBirth, Gender gender)
    {
        FullName = fullName;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        AgeGroup = AgeGroup.CalculateAgeGroup(dateOfBirth);
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FullName;
        yield return DateOfBirth;
        yield return Gender;
        yield return AgeGroup;
    }
}