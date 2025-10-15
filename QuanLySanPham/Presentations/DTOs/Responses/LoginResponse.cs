namespace QuanLySanPham.Presentations.DTOs.Responses;

public class LoginResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }

    public LoginResponse(string token, string refreshToken, DateTime refreshTokenExpiresAt)
    {
        Token = token;
        RefreshToken = refreshToken;
        RefreshTokenExpiresAt = refreshTokenExpiresAt;
    }
}