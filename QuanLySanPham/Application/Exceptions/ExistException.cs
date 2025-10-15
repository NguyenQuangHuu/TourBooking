namespace QuanLySanPham.Application.Exceptions;

public class ExistException : Exception
{
    private string Messages { get; set; }

    public ExistException(string message) : base(message)
    {
        Messages = message;
    }
}