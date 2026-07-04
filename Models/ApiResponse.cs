namespace ProductManagementAPI.Models
{
    /// <summary>
    /// Standard response envelope returned by all API endpoints.
    /// </summary>
    /// <typeparam name="T">The type of the response payload.</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates whether the request was processed successfully.
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// The response payload. <c>null</c> when the request failed.
        /// </summary>
        public T? Data { get; init; }

        /// <summary>
        /// A human-readable message describing the outcome.
        /// </summary>
        public string? Message { get; init; }

        /// <summary>
        /// Validation or processing error messages. Empty on success.
        /// </summary>
        public IReadOnlyList<string> Errors { get; init; } = [];

        /// <summary>
        /// Creates a successful response wrapping the given data.
        /// </summary>
        public static ApiResponse<T> SuccessResult(T data, string? message = null) =>
            new() { Success = true, Data = data, Message = message };

        /// <summary>
        /// Creates a failure response with a single error message.
        /// </summary>
        public static ApiResponse<T> FailResult(string error) =>
            new() { Success = false, Errors = [error] };

        /// <summary>
        /// Creates a failure response with multiple error messages.
        /// </summary>
        public static ApiResponse<T> FailResult(IReadOnlyList<string> errors) =>
            new() { Success = false, Errors = errors };
    }
}
