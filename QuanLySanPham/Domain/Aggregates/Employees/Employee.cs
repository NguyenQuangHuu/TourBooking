using QuanLySanPham.Domain.Commons;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Aggregates.Employees;

public class Employee : BaseEntity<EmployeeId>,IAggregateRoot
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public string IdentityCardNumber { get; set; }
    public Gender Gender { get; set; }
    public UserId? UserId { get; set; }
    
    public string Department { get; set; }
    
    public string Address { get; set; }

    public Role Role { get; private set; }

    public Employee(string fullName, string email, string phoneNumber, string identityCardNumber, Gender gender,DateOnly dateOfBirth, string address)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        IdentityCardNumber = identityCardNumber;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        Address = address;
        Role = Role.Employee;
    }

    public void SetAccount(UserId userId)
    {
        UserId = userId;
    }

    public void ChangeRole(Role role)
    {
        if (!Role.Equals(role))
        {
            Role  = role;
        }
    }

    public void UpdateEmployee(Employee employee)
    {
        FullName = employee.FullName;
        Email = employee.Email;
        PhoneNumber = employee.PhoneNumber;
        IdentityCardNumber = employee.IdentityCardNumber;
        Gender = employee.Gender;
    }
}