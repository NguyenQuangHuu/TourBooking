namespace QuanLySanPham.Application.Exceptions;

public class AuthException : Exception
{
    public string Message { get; set; }

    public AuthException(string message) : base(message)
    {
        Message = message;
    }
}