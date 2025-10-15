using QuanLySanPham.Domain.Commons;
using System;
using System.Net.Mail;

namespace QuanLySanPham.Application.DTO.Auth;

public class EmailDto : ValueObject
{
    public string Value { get; set; }

    public EmailDto(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("EmailDto can not be null");

        if (value.Length < 6) throw new ArgumentException("EmailDto must contain at least 6 characters");

        try
        {
            var email = new MailAddress(value);
            Value = email.Address;
        }
        catch (FormatException)
        {
            throw new ArgumentException("EmailDto is not valid");
        }
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(EmailDto emailDto)
    {
        return emailDto.Value;
    }

    public static explicit operator EmailDto(string value)
    {
        return new EmailDto(value);
    }
}