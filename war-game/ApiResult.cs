namespace war_game {
    public class ApiResult {
        public bool isSuccess { get; set; }
        public uint statusCode { get; set; }
        public string? message { get; set; }

        public static ApiResult Success(uint status = 0, string? message = null) =>
            new() { isSuccess = true, statusCode = status, message = message };

        public static ApiResult Failure(uint status = 0, string? message = null) =>
            new() { isSuccess = false, statusCode = status, message = message };
    }
}
