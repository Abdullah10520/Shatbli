namespace Shatabli.Core.Domain.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Errors = new Dictionary<string, List<string>>(),
                Timestamp = DateTime.UtcNow
            };
        }

        public static ApiResponse<T> FailureResponse(string message, Dictionary<string, List<string>> errors)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Errors = errors,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}