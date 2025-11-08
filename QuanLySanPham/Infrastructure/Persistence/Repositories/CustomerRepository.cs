using System.Data;
using Npgsql;
using QuanLySanPham.Application.Exceptions;
using QuanLySanPham.Domain.Aggregates.Customers;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Infrastructure.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IUnitOfWork _unitOfWork;
    public CustomerRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Customer> CreateNewCustomerInformationAsync(Customer customer, CancellationToken token)
    {
        try
        {
            string sql = "insert into customers(display_name,gender,address,identity_card_no,date_of_birth,user_id)"
                         + "VALUES (@DisplayName,@Gender,@Address,@IdentityCardNo,@Dob,@UserId)"
                         + "returning id";
            await using var command = new NpgsqlCommand(sql, _unitOfWork.Connection, _unitOfWork.Transaction);
            command.Parameters.Add(new NpgsqlParameter("@DisplayName", customer.DisplayName));
            command.Parameters.Add(new NpgsqlParameter("@Gender", customer.Gender));
            command.Parameters.Add(new NpgsqlParameter("@Address", customer.Address));
            command.Parameters.Add(new NpgsqlParameter("@IdentityCardNo", customer.IdentityCard));
            command.Parameters.Add(new NpgsqlParameter("@Dob", customer.BirthDate));
            command.Parameters.Add(new NpgsqlParameter("@UserId", customer.UserId.Value));
            var result = await command.ExecuteScalarAsync(token);
            if (result is null)
            {
                throw new InvalidOperationException("No returning id");
            }

            CustomerId newCustomerId = CustomerId.From((Guid)result);
            customer.Id = newCustomerId;
            return customer;
        }
        catch (PostgresException ex)
        {
            throw new DomainException($"An error occurred creating the customer. {ex.Message}");
        }
    }

    public async Task<Customer?> GetCustomerByIdAsync(CustomerId id, CancellationToken token)
    {
        string sql = "select c.id, c.display_name, c.gender, c.address, c.identity_card_no, c.date_of_birth, c.user_id from customers c where c.id = @Id";
        await using var cmd = new NpgsqlCommand(sql, _unitOfWork.Connection);
        cmd.Parameters.Add(new NpgsqlParameter("@Id", id.Value));
        await using var reader = await cmd.ExecuteReaderAsync(token);
        if (!await reader.ReadAsync(token))
        {
            throw new NotFoundException("Customer not found");
        }
        return new Customer
        {
            Id = CustomerId.From(reader.GetGuid(0)),
            DisplayName = reader.GetString(1),
            BirthDate = DateOnly.FromDateTime(reader.GetDateTime(5)),
            Gender = (Gender)reader.GetString(2),
            Address = reader.GetString(3),
            IdentityCard = reader.GetString(4),
            UserId = UserId.From(reader.GetGuid(6))
        };
    }
}