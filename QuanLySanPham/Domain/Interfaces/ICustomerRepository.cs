using QuanLySanPham.Domain.Aggregates.Customers;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<Customer> CreateNewCustomerInformationAsync(Customer customer, CancellationToken token);
    Task<Customer?> GetCustomerByIdAsync(CustomerId id, CancellationToken token);
}