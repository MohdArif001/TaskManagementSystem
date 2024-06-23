using System.ComponentModel.DataAnnotations;

namespace TaskDashboard.Models
{
    public class SignUpModel
    {
        public string FirstName { get; set; }


        public string LastName { get; set; }


        [EmailAddress]
        public string EmailId { get; set; }


        public string Password { get; set; }


        [Compare("Password", ErrorMessage = "Password Mismatched")]
        public string ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }
    }
}
