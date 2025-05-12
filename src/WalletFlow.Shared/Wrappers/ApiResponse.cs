namespace WalletFlow.Shared.Wrappers;

public class ApiResponse<T>(T data, string? message = null)
{
    public T Data { get; set; } = data;
    public bool Success { get; set; } = true;
    public string? Message { get; set; } = message;

    public ApiResponse(string message) : this(default(T)!, message)
    {
        Success = false;
    }
}