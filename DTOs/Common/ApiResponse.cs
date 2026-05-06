namespace Test.DTOs.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;

        // 성공 응답
        public static ApiResponse<T> SuccessResponse(T data, string message = "요청 성공")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        // 실패 응답
        public static ApiResponse<T> FailResponse(string message = "요청 실패")
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default,
                Message = message
            };
        }
    }
}