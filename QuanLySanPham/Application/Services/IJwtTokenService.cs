using QuanLySanPham.Domain.Aggregates.Auth;
using QuanLySanPham.Domain.Aggregates.Employees;

namespace QuanLySanPham.Application.Services;

public interface IJwtTokenService
{
    string GenerateJwtTokenForCustomer(User user);
    
    string GenerateJwtTokenForEmployee(User user, Employee employee);

    string GenerateRefreshToken();
}