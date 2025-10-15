using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Application.DTO.Auth;

public class UsernameDto : ValueObject
{
    public string Value { get; set; }

    public UsernameDto(string value)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentException("UsernameDto cannot be null or empty");

        if (value.Length < 6) throw new ArgumentException("UsernameDto must be at least 6 characters");
        Value = value.Trim();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(UsernameDto usernameDto)
    {
        return usernameDto.Value;
    }

    public static explicit operator UsernameDto(string username)
    {
        return new UsernameDto(username);
    }
}