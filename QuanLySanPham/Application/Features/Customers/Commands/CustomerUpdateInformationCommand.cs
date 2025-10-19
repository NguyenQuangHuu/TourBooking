using MediatR;
using QuanLySanPham.Domain.Aggregates.Customers;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Application.Features.Customers.Commands;

public record CustomerUpdateInformationCommand(Customer Customer) : IRequest<CustomerId?>;

public class CustomerUpdateInformationCommandHandler : IRequestHandler<CustomerUpdateInformationCommand, CustomerId?>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerUpdateInformationCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<CustomerId?> Handle(CustomerUpdateInformationCommand request, CancellationToken ct)
    {
        await _unitOfWork.BeginAsync(ct);
        var result = await _customerRepository.CreateNewCustomerInformationAsync(request.Customer, ct);
        await _unitOfWork.CommitAsync(ct);
        return result;
    }
}