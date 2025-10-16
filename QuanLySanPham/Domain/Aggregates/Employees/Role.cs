using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Aggregates.Employees;

public class Role : ValueObject
{
    public string Value { get; set; }

    public Role(string roleName)
    {
        Value = roleName;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static Role Employee = new Role("Employee");
    public static Role Manager = new Role("Manager");
    public static Role Admin = new Role("Admin");
}