using UserManagementService.Model;
using UserManagementService.Repository.Interfaces;

namespace UserManagementService.Repository.Services
{
    public class User:IUser
    {
        private readonly UserDbContext _context;
        public User(UserDbContext context)
        {
            _context = context;
        }

        public bool CheckEmail(UserRequest userRequest)
        {
            try
            {
                bool checkEmail = _context.UserDetails.Where(x => x.EmailId == userRequest.EmailId).Any();
                return checkEmail;
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        public UserDetail Login(LoginRequest loginRequest)
        {
            try
            {
                var userDetails = _context.UserDetails.Where(x => x.EmailId == loginRequest.Email && x.Password == loginRequest.Password).FirstOrDefault();
                return userDetails;
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        public bool SignUp(UserRequest userRequest)
        {
            try
            {
                UserDetail userDetail = new UserDetail();
                userDetail.EmailId = userRequest.EmailId;
                userDetail.FirstName = userRequest.FirstName;
                userDetail.LastName = userRequest.LastName;
                userDetail.PhoneNumber = userRequest.PhoneNumber;
                userDetail.Password = userRequest.Password;
                userDetail.CreatedDate = DateTime.UtcNow;
                userDetail.ModifiedDate= DateTime.UtcNow;
                var data =  _context.UserDetails.Add(userDetail);
                if (data != null)
                {
                    var check = _context.SaveChanges();
                    if (check > 0)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
