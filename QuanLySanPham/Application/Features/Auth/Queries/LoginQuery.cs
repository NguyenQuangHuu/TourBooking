using MediatR;
using QuanLySanPham.Application.Exceptions;
using QuanLySanPham.Application.Services;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Presentations.DTOs.Responses;

namespace QuanLySanPham.Application.Features.Auth.Queries;

public record LoginQuery(string Username, string Password) : IRequest<LoginResponse>;

public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResponse>
{
    private readonly IAuthRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginQueryHandler(IAuthRepository repository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponse> Handle(LoginQuery request, CancellationToken ct)
    {
        await _unitOfWork.BeginTransactionAsync(ct);
        var user = await _repository.GetUserByUsernameAsync(request.Username, ct);
        if (user is null) throw new AuthException("Username is not exist");

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new AuthException("Password is incorrect");
        
        var token = _jwtTokenService.GenerateJwtTokenForCustomer(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddDays(7);
        user.GenerateRefreshToken(refreshToken, expiresAt);
        await _repository.UpdateUserAsync(user, ct);
        await _unitOfWork.CommitAsync(ct);
        return new LoginResponse(token, refreshToken, expiresAt);
    }
}