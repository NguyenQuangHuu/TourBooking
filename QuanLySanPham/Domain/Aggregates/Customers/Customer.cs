using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Aggregates.Customers;

public class Customer : BaseEntity<CustomerId>,IAggregateRoot
{
    public string? DisplayName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public Gender? Gender { get; set; }
    public string? IdentityCard { get; set; }
    public string? Address { get; set; }
    public UserId UserId { get; set; }
    public Customer(){}
    public Customer(string displayName, DateOnly birthDate, Gender gender, string identityCard, string address,UserId userId)
    {
        DisplayName = displayName;
        BirthDate = birthDate;
        Gender = gender;
        IdentityCard = identityCard;
        Address = address;
        UserId = userId;
    }
    public void UpdateCustomer(Customer customer)
    {
        DisplayName = customer.DisplayName;
        BirthDate = customer.BirthDate;
        Gender = customer.Gender;
        IdentityCard = customer.IdentityCard;
        Address = customer.Address;
    }
}