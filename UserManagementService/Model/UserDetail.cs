using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Model
{
    public class UserDetail
    {
        [Key]
        public int UserId { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
