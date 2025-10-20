using Npgsql;
using QuanLySanPham.Domain.Aggregates.Employees;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Infrastructure.Persistence.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IUnitOfWork _unitOfWork;
    public EmployeeRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Employee> CreateEmployeeAsync(Employee employee, CancellationToken ct)
    {
        string sql =
            "insert into employees(full_name, gender, address, identity_card_no, department_name, date_of_birth, user_id, role_name)"
            + " values (@FullName,@Gender,@Address,@IdentityNo,@DepartmentName,@Dob,@UserId,@RoleName) returning id";
        await using var cmd = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        cmd.Parameters.Add(new NpgsqlParameter("@FullName", employee.FullName));
        cmd.Parameters.Add(new NpgsqlParameter("@Gender", employee.Gender));
        cmd.Parameters.Add(new NpgsqlParameter("@Address", employee.Address));
        cmd.Parameters.Add(new NpgsqlParameter("@IdentityNo", employee.IdentityCardNumber));
        cmd.Parameters.Add(new NpgsqlParameter("@DepartmentName", employee.Department));
        cmd.Parameters.Add(new NpgsqlParameter("@Dob", employee.DateOfBirth));
        cmd.Parameters.Add(new NpgsqlParameter("@UserId", employee.UserId));
        cmd.Parameters.Add(new NpgsqlParameter("@RoleName", employee.Role));
        var result = await cmd.ExecuteScalarAsync(ct);
        if (result is null)
        {
            throw new InvalidOperationException("Could not create employee");
        }
        EmployeeId id = EmployeeId.From((Guid)result);
        employee.Id = id;
        return employee;
    }
}