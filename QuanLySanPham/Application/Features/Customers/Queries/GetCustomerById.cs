using MediatR;
using QuanLySanPham.Application.Exceptions;
using QuanLySanPham.Domain.Aggregates.Customers;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects.Ids;
using QuanLySanPham.Presentations.DTOs.Responses;

namespace QuanLySanPham.Application.Features.Customers.Queries;

public class GetCustomerById:IRequest<Result<Customer>>
{
    public CustomerId CustomerId { get; set; }

    public GetCustomerById(Guid id)
    {
        CustomerId = CustomerId.From(id);
    }
}

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerById, Result<Customer>>
{
    private readonly IUnitOfWork  _unitOfWork;
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
    {
        _unitOfWork = unitOfWork;
        _customerRepository = customerRepository;
    }
    public async Task<Result<Customer>> Handle(GetCustomerById request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            Customer customer = await _customerRepository.GetCustomerByIdAsync(request.CustomerId, cancellationToken);
            return Result<Customer>.Success(customer, StatusCodes.Status200OK);
        }
        catch (NotFoundException ex)
        {
            return Result<Customer>.Failure(ex.Message, StatusCodes.Status404NotFound);
        }
    }
}