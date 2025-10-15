using MediatR;
using QuanLySanPham.Application.Exceptions;
using QuanLySanPham.Application.Services;
using QuanLySanPham.Domain.Interfaces;
using QuanLySanPham.Presentations.DTOs.Responses;

namespace QuanLySanPham.Application.Features.Auth;

public record GenerateTokenCommand(string RefreshToken) : IRequest<LoginResponse>;

public class GenerateTokenCommandHandler : IRequestHandler<GenerateTokenCommand, LoginResponse>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IAuthRepository _authRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GenerateTokenCommandHandler(IJwtTokenService jwtTokenService, IAuthRepository authRepository,
        IUnitOfWork unitOfWork)
    {
        _jwtTokenService = jwtTokenService;
        _authRepository = authRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginResponse> Handle(GenerateTokenCommand request, CancellationToken ct)
    {
        await _unitOfWork.BeginAsync(ct);
        var user = await _authRepository.GetUserByRefreshTokenAsync(request.RefreshToken, ct);
        if (user is null || user.RefreshTokenExpiration <= DateTime.Now) throw new AuthException("Invalid Credentials");
        if (!user.RefreshToken.Equals(request.RefreshToken)) throw new AuthException("Invalid Credentials");
        var newToken = _jwtTokenService.GenerateJwtToken(user);
        var newRfToken = _jwtTokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddDays(7);
        await _authRepository.SaveRefreshTokenAsync(newRfToken, expiresAt, user.Id, ct);
        await _unitOfWork.CommitAsync(ct);
        var res = new LoginResponse(newToken, newRfToken, expiresAt);
        return res;
    }
}