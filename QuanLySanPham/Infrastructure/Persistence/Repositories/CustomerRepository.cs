using Npgsql;
using QuanLySanPham.Domain.Aggregates.Customers;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Infrastructure.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IUnitOfWork _unitOfWork;
    public CustomerRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<CustomerId?> CreateNewCustomerInformationAsync(Customer customer, CancellationToken token)
    {
        string sql = "insert into customers(display_name,gender,address,identity_card_no,date_of_birth,user_id)"
                     +"VALUES (@DisplayName,@Gender,@Address,@IdentityCardNo,@Dob,@UserId)"
                     +"returning id";
        await using var command = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
        command.Parameters.Add(new NpgsqlParameter("@DisplayName",customer.DisplayName));
        command.Parameters.Add(new NpgsqlParameter("@Gender",customer.Gender));
        command.Parameters.Add(new NpgsqlParameter("@Address",customer.Address));
        command.Parameters.Add(new NpgsqlParameter("@IdentityCardNo",customer.IdentityCard));
        command.Parameters.Add(new NpgsqlParameter("@Dob",customer.BirthDate));
        command.Parameters.Add(new NpgsqlParameter("@UserId",customer.UserId.Value));
        var result = await command.ExecuteScalarAsync(token);
        if (result is not null)
        {
            return CustomerId.From((Guid)result);
        }

        return null;
    }
}