namespace QuanLySanPham.Domain.Exceptions;

public class DomainException : Exception
{
    private string Message { get; set; }

    public DomainException(string message) : base(message)
    {
        Message = message;
    }
}