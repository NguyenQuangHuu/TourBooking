using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects;

public class AccountType : ValueObject
{
    public static AccountType Customer => new("Customer");
    public static AccountType Employee => new("Employee");
    private string Value { get; set; }

    private AccountType(string value)
    {
        Value = value;
    }

    public static implicit operator string(AccountType type)
    {
        return type.Value;
    }

    public static explicit operator AccountType(string value)
    {
        return new AccountType(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}