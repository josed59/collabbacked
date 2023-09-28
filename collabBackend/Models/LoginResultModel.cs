namespace collabBackend.Models
{
    public class LoginResultModel
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Error { get; set; }
        public UserModel User { get; set; }
    }
}
