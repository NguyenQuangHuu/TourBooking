using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Aggregates.Auth;

public class User : BaseEntity<UserId>, IAggregateRoot
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public AccountType Type { get; init; }

    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }

    public User()
    {
    }

    public User(string username, string passwordHashed, string email, string phoneNumber, AccountType type)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Username can not be null or contains space character");
        if (string.IsNullOrWhiteSpace(passwordHashed))
            throw new DomainException("Password is invalid");
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email can not be null or contains space character");
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new DomainException("Phone number can not be null or contains space character");
        Username = username;
        PasswordHash = passwordHashed;
        Email = email;
        PhoneNumber = phoneNumber;
        Type = AccountType.Customer;
    }
}