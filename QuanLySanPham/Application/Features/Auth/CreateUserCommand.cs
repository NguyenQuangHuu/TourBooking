using MediatR;
using QuanLySanPham.Application.Services;
using QuanLySanPham.Domain.Aggregates.Auth;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Domain.ValueObjects;
using QuanLySanPham.Application.DTO.Auth;
using QuanLySanPham.Application.Exceptions;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Application.Features.Auth;

public record CreateUserCommand(
    UsernameDto UsernameDto,
    PasswordDto PasswordDto,
    EmailDto EmailDto,
    PhoneNumberDto PhoneNumberDto) : IRequest<UserId?>;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserId?>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IPasswordHasher passwordHasher, IAuthRepository repository, IUnitOfWork unitOfWork)
    {
        _passwordHasher = passwordHasher;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserId?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        var exists = await _repository.GetUserByUsernameAsync(request.UsernameDto, cancellationToken);
        if (exists is not null) throw new ExistException("User with that username already exists");

        var passwordHashed = _passwordHasher.Hash(request.PasswordDto.Value);
        var user = new User(request.UsernameDto, passwordHashed, request.EmailDto, request.PhoneNumberDto,
            AccountType.Customer);
        var result = await _repository.CreateUserAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return result;
    }
}