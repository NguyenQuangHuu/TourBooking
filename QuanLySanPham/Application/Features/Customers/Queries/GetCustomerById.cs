using MediatR;
using QuanLySanPham.Application.Exceptions;
using QuanLySanPham.Domain.Aggregates.Customers;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Presentations.DTOs.Responses;

namespace QuanLySanPham.Application.Features.Customers.Queries;

public class GetCustomerById:IRequest<ApiResponse<Customer>>
{
    public CustomerId CustomerId { get; set; }

    public GetCustomerById(Guid id)
    {
        CustomerId = CustomerId.From(id);
    }
}

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerById, ApiResponse<Customer>>
{
    private readonly IUnitOfWork  _unitOfWork;
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
    {
        _unitOfWork = unitOfWork;
        _customerRepository = customerRepository;
    }
    public async Task<ApiResponse<Customer>> Handle(GetCustomerById request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginAsync(cancellationToken);
            Customer customer = await _customerRepository.GetCustomerByIdAsync(request.CustomerId, cancellationToken);
            return ApiResponse<Customer>.Ok(customer);
        }
        catch (NotFoundException ex)
        {
            return ApiResponse<Customer>.Failure(ex.Message);
        }
    }
}