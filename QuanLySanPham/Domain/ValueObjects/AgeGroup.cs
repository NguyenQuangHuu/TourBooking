using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.Exceptions;

namespace QuanLySanPham.Domain.ValueObjects;

public class AgeGroup : ValueObject
{
    private static readonly int MIN_AGE = 0;
    private static readonly int MAX_AGE = 70;
    public static AgeGroup Infant = new AgeGroup("infant","trẻ sơ sinh",0,3);
    public static AgeGroup Child = new AgeGroup("child", "trẻ em", 3, 10);
    public static AgeGroup Teen = new AgeGroup("teen", "thanh thiếu niên", 10, 17);
    public static AgeGroup Adult = new AgeGroup("adult", "người lớn", 18, 55);
    public static AgeGroup Senior = new AgeGroup("senior", "người già", 55, 70);
    
    public string AgeType { get; set; }
    public string AgeName { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    private AgeGroup()
    {
        
    }
    private AgeGroup(string type,string name, int minAge, int maxAge)
    {
        AgeType = type;
        AgeName = name;
        MinAge = minAge;
        MaxAge = maxAge;
    }

    public static AgeGroup CalculateAgeGroup(DateOnly dateOfBirth)
    {
        var today =  DateOnly.FromDateTime(DateTime.Now);
        var age = today.Year - dateOfBirth.Year;
        if (today > dateOfBirth)
        {
            age--;
        }
        return (age) switch
        {
            < 0 => throw new DomainException("Tuổi không hợp lệ."),
            < 3 => Infant,
            < 10 => Child,
            < 18 => Teen,
            < 55 => Adult,
            < 70 => Senior,
            _ => throw new DomainException("Tuổi đã vượt quá qui định.")
        };
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return AgeType;
        yield return AgeName;
        yield return MinAge;
        yield return MaxAge;
    }
}