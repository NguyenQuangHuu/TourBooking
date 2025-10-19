using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLySanPham.Application.Features.Auth;
using QuanLySanPham.Application.DTO.Auth;
using QuanLySanPham.Application.Exceptions;
using QuanLySanPham.Application.Features.Auth.Queries;
using QuanLySanPham.Domain.Exceptions;
using QuanLySanPham.Presentations.DTOs.Requests;
using QuanLySanPham.Presentations.DTOs.Responses;

namespace QuanLySanPham.Presentations.APIs;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuthController : Controller
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sign-up")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request,
        CancellationToken ct = default)
    {
        try
        {
            var username = new UsernameDto(request.Username);
            var password = new PasswordDto(request.Password);
            var email = new EmailDto(request.Email);
            var phone = new PhoneNumberDto(request.PhoneNumber);
            var command = new CreateUserCommand(username, password, email, phone);
            var result = await _mediator.Send(command, ct);
            return Ok(result);
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ExistException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("sign-in")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] LoginRequest request, CancellationToken ct)
    {
        try
        {
            var login = new LoginQuery(request.Username, request.Password);
            var result = await _mediator.Send(login, ct);
            var response = ApiResponse<LoginResponse>.Ok(result);
            return Ok(response);
        }
        catch (AuthException ex)
        {
            return BadRequest(ApiResponse<LoginResponse>.Failure(ex.Message));
        }
    }

    [HttpPost("rf-token")]
    [Authorize(Policy="CustomerOrEmployee")]
    public async Task<IActionResult> GenerateTokenByRfToken([FromBody] RefreshTokenRequest rfToken)
    {
        try
        {
            var invokeToken = new GenerateTokenCommand(rfToken.RefreshToken);
            var result = await _mediator.Send(invokeToken, CancellationToken.None);
            return Ok(ApiResponse<LoginResponse>.Ok(result));
        }
        catch (AuthException ex)
        {
            return BadRequest(ApiResponse<LoginResponse>.Failure(ex.Message));
        }
    }

    [HttpGet("sign-out")]
    public async Task<IActionResult> SignOut([FromBody] SignOutRequest signOut, CancellationToken ct)
    {
        var result =  await _mediator.Send(signOut, ct);
        return Ok(ApiResponse<string>.Ok("Log out success"));
    }
}