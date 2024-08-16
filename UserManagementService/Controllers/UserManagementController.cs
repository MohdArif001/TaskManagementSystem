using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagementService.Model;
using UserManagementService.Repository.Interfaces;
using UserManagementService.Repository.Services;

namespace UserManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUser _user;
        private readonly IConfiguration _configuration;
        public UserManagementController(IUser user, IConfiguration configuration)
        {
            _user = user;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("SignUp")]
        public ActionResult SignUp(UserRequest userRequest)
        {
            if (ModelState.IsValid)
            {
                bool checkEmail = _user.CheckEmail(userRequest);
                if (checkEmail)
                {
                    return Ok(new
                    {
                        Message = "Email already exists"
                    });
                }
                else
                {
                    var isSignUp = _user.SignUp(userRequest);
                    if (isSignUp)
                    {
                        return Ok(new
                        {
                            Message = "Registered Successfully"
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            Message = "User Not Registered"
                        });
                    }
                }
            }
            else
            {
                return BadRequest(new
                {
                    Message = "User Not Registered"
                });
            }
        }
        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {

                var userDetail = _user.Login(loginRequest);
                if (userDetail != null)
                {
                    UserDetailModel userDetailModel = new UserDetailModel();
                    userDetailModel.UserId = userDetail.UserId;
                    userDetailModel.EmailId = userDetail.EmailId;
                    userDetailModel.LastName = userDetail.LastName;
                    userDetailModel.FirstName = userDetail.FirstName;
                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, userDetail.EmailId),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                };
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokeOptions = new JwtSecurityToken(issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"], authClaims,
                        expires: DateTime.Now.AddHours(Convert.ToDouble(_configuration["JWT:TokenValidityInHours"])), signingCredentials: signinCredentials);
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    return Ok(new JWTTokenResponse
                    {
                        Token = tokenString,
                        Message = "Login Successfully",
                        UserDetail = userDetailModel
                    });

                }
                else
                {
                    return Ok(new
                    {
                        Message = "Invalid user"
                    });
                }
            }
            else return BadRequest();
        }

    }
}
