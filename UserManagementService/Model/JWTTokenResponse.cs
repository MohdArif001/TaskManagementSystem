namespace UserManagementService.Model
{
    public class JWTTokenResponse
    {
        public string Token { get; set; }
        public string Message { get; set; }
        public UserDetailModel UserDetail { get; set; }
    }
}
