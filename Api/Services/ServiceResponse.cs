namespace Api.Services
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }

}
