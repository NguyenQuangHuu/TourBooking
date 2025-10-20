using QuanLySanPham.Domain.Aggregates.Employees;

namespace QuanLySanPham.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee> CreateEmployeeAsync(Employee employee,CancellationToken ct);
}