namespace Test.DTOs.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;

        public List<string>? Errors { get; set; }

        // 성공 응답
        public static ApiResponse<T> SuccessResponse(T data, string message = "요청 성공")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message,
                Errors = null
            };
        }

        // 일반 실패 응답
        public static ApiResponse<T> FailResponse(string message = "요청 실패")
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default,
                Message = message,
                Errors = null
            };
        }

        // 🔥 Validation 등 상세 에러용
        public static ApiResponse<T> FailResponse(string message, List<string> errors)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default,
                Message = message,
                Errors = errors
            };
        }
    }
}