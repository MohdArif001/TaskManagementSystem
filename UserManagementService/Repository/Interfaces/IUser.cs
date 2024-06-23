using UserManagementService.Model;

namespace UserManagementService.Repository.Interfaces
{
    public interface IUser
    {
        public bool SignUp(UserRequest userRequest);
        public bool CheckEmail(UserRequest userRequest);
        public UserDetail Login (LoginRequest loginRequest);
    }
}
