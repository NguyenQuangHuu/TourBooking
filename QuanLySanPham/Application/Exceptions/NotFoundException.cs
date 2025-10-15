namespace QuanLySanPham.Application.Exceptions;

public class NotFoundException : Exception
{
    public string Message { get; set; }

    public NotFoundException(string message) : base(message)
    {
        Message = message;
    }
}