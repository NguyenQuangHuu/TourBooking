using QuanLySanPham.Domain.Commons;

namespace QuanLySanPham.Domain.ValueObjects;

public class PassengerContact :ValueObject
{
    public string Phone { get; set; }
    public string Address  { get; set; }
    public string Email { get; set; }

    public PassengerContact(string phone, string address, string email)
    {
        Phone = phone;
        Address = address;
        Email = email;
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Phone;
        yield return Address;
        yield return Email;
    }
}