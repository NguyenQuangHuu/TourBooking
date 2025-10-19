using QuanLySanPham.Domain.Aggregates.Customers;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<CustomerId?> CreateNewCustomerInformationAsync(Customer customer, CancellationToken token);
}