using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Aggregates.Customers;

public class Customer : BaseEntity<CustomerId>
{
    public string DisplayName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public Gender? Gender { get; set; }
    public string? IdentityCard { get; set; }
    public string? Address { get; set; }
}