namespace QuanLySanPham.Infrastructure.Exceptions;

public class InfrastructureException : Exception
{
    public string Message { get; set; }

    public InfrastructureException(string message) : base(message)
    {
        Message = message;
    }
}