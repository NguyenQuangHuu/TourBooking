namespace QuanLySanPham.Presentations.DTOs.Responses;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    
    public int StatusCode { get; set; }
    public T? Data { get; set; }

    private Result(bool isSuccess, string message,int statusCode, T? data)
    {
        IsSuccess = isSuccess;
        Message = message;
        StatusCode = statusCode;
        Data = data;
    }

    public static Result<T> Success(T data,int statusCode, string message = "Thành công")
    {
        return new Result<T>(true, message, statusCode,data);
    }

    public static Result<T> Failure(string message,int statusCode)
    {
        return new Result<T>(false, message, statusCode,default);
    }
}