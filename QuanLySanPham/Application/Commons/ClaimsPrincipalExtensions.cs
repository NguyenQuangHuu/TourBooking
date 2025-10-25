using System.Security.Claims;
using QuanLySanPham.Application.Exceptions;
using QuanLySanPham.Domain.ValueObjects.Ids;

namespace QuanLySanPham.Application.Commons;

public static class ClaimsPrincipalExtensions
{
    public static UserId GetUserIdPrincipal(this ClaimsPrincipal principal)
    {
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User ID không hợp lệ");
        }
        return UserId.From(Guid.Parse(userId));
    }
}