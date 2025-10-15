namespace QuanLySanPham.Presentations.DTOs.Responses;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public ApiResponse(bool isSuccess, string message, T? data)
    {
        IsSuccess = isSuccess;
        Message = message;
        Data = data;
    }

    public static ApiResponse<T> Ok(T data, string message = "Thành công")
    {
        return new ApiResponse<T>(true, message, data);
    }

    public static ApiResponse<T> Failure(string message)
    {
        return new ApiResponse<T>(false, message, default);
    }
}