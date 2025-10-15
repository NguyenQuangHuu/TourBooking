using System.Text.RegularExpressions;
using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Application.DTO.Auth;

public class PhoneNumberDto : ValueObject
{
    public string Value { get; set; }

    public PhoneNumberDto(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Phone number can not be empty");
        var pattern = @"^(?:\+84|0)(3|5|7|8|9)\d{8}$";
        if (!Regex.IsMatch(value, pattern))
            throw new ArgumentException("Phone number is not valid or not vietnam phone number");
        Value = value;
    }

    public static implicit operator string(PhoneNumberDto phoneNumberDto)
    {
        return phoneNumberDto.Value;
    }

    public static explicit operator PhoneNumberDto(string phoneNumber)
    {
        return new PhoneNumberDto(phoneNumber);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}