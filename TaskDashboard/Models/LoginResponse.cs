namespace TaskDashboard.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Message { get; set; }
        public UserDetail UserDetail { get; set; }
    }
}
