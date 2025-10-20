using MediatR;
using QuanLySanPham.Domain.Aggregates.Customers;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Application.Features.Customers.Commands;

public record CustomerUpdateInformationCommand(string DisplayName, DateOnly DateOfBirth, Gender Gender,
    string IdentityCardNumber, string Address,UserId UserId) : IRequest<Customer>;

public class CustomerUpdateInformationCommandHandler : IRequestHandler<CustomerUpdateInformationCommand, Customer>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerUpdateInformationCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Customer> Handle(CustomerUpdateInformationCommand request, CancellationToken ct)
    {
        await _unitOfWork.BeginAsync(ct);
        Customer customer = new Customer(request.DisplayName, request.DateOfBirth, request.Gender,
            request.IdentityCardNumber, request.Address, request.UserId);
        var result = await _customerRepository.CreateNewCustomerInformationAsync(customer, ct);
        await _unitOfWork.CommitAsync(ct);
        return result;
    }
}