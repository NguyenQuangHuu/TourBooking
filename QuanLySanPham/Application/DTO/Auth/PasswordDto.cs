using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Application.DTO.Auth;

public class PasswordDto : ValueObject
{
    public string Value { get; private set; }

    public PasswordDto(string value)
    {
        IsValidPassword(value);
        Value = value;
    }

    public static implicit operator string(PasswordDto passwordDto)
    {
        return passwordDto.Value;
    }

    public static explicit operator PasswordDto(string value)
    {
        return new PasswordDto(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    private void IsValidPassword(string password)
    {
        if (string.IsNullOrEmpty(password)) throw new ArgumentException("PasswordDto must have a value");

        if (password.Length < 6) throw new ArgumentException("PasswordDto is too short");

        if (password.Length >= 50) throw new ArgumentException("PasswordDto is too long");

        // if (password.Any(c=>"0123456789".Contains(c)))
        // {
        //     throw new ArgumentException("PasswordDto must contains at least digit");
        // }

        // if (password.Any(char.IsUpper))
        // {
        //     throw new ArgumentException("PasswordDto must contains at least one upper character");
        // }
        //
        // if (password.Any(char.IsLower))
        // {
        //     throw new ArgumentException("PasswordDto must contains at least one lower character");
        // }
        //
        // if (password.Any(c => "!@#$%^&*()".Contains(c)))
        // {
        //     throw new ArgumentException("PasswordDto must contains at least one punctuation");
        // }
    }
}