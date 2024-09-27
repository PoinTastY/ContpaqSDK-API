namespace Application.DTOs
{
    public class ApiResponse
    {
        public string Message { get; set; } = "No message";
        public bool Success { get; set; }
        public object Data { get; set; } = null!;
        public string Error { get; set; } = "No error";
    }
}
