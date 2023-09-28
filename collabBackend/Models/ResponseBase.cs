namespace collabBackend.Models
{
    public class ResponseBase<T>
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }
    }
}
