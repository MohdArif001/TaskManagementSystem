namespace UserManagementService.Model
{
    public class UserRequest
    {
        public string EmailId { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
