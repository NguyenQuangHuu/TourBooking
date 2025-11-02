using MediatR;
using QuanLySanPham.Application.Exceptions;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Presentations.DTOs.Responses;

namespace QuanLySanPham.Application.Features.Commands.Auth;

public record CustomerSignOutCommand(string Username): IRequest<Result<string>>;

public class CustomerSignOutCommandHandler : IRequestHandler<CustomerSignOutCommand, Result<string>>
{
    private readonly IAuthRepository  _authRepository;
    private readonly IUnitOfWork  _unitOfWork;

    public CustomerSignOutCommandHandler(IAuthRepository authRepository, IUnitOfWork unitOfWork)
    {
        _authRepository = authRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<string>> Handle(CustomerSignOutCommand command, CancellationToken ct)
    {
        await _unitOfWork.BeginAsync(ct);
        try
        {
            var user = await _authRepository.GetUserByUsernameAsync(command.Username, ct);
            if (user is null)
            {
                throw new AuthException("Invalid User");
            }

            user.RevokeRefreshToken();
            await _authRepository.UpdateUserAsync(user, ct);
            await _unitOfWork.CommitAsync(ct);
            return  Result<string>.Success("Logout successfully",200);
        }
        catch (DomainException ex)
        {
            await _unitOfWork.RollbackAsync(ct);
            return Result<string>.Failure(ex.Message,StatusCodes.Status403Forbidden);
        }
    }
}