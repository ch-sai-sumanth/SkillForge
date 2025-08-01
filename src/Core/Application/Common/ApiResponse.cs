namespace Application.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static ApiResponse<T> SuccessResult(T data, string message = "")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }

    public static ApiResponse<T> ErrorResult(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }

    public static ApiResponse<T> ErrorResult(List<string> errors)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = "Validation failed",
            Errors = errors
        };
    }
}

public static class ApiResponse
{
    public static ApiResponse<object> Success(string message = "Operation completed successfully")
    {
        return new ApiResponse<object>
        {
            Success = true,
            Message = message
        };
    }

    public static ApiResponse<object> Error(string message, List<string>? errors = null)
    {
        return new ApiResponse<object>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}