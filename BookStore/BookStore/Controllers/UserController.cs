using Business_Layer.Interface;
using Common_Layer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BookStore.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;

        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }

        // Registration
        [HttpPost("register")]
        public IActionResult Registration(UserRegModel UserReg)
        {
            try
            {
                UserRegModel userData = this.userBL.Registration(UserReg);
                if (userData != null)
                {
                    return this.Ok(new { Success = true, message = "User Added Sucessfully", Response = userData });
                }
                return this.Ok(new { Success = true, message = "Sorry! User Already Exists" });
            }
            catch (System.Exception ex)
            {

                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }

        // LOGIN
        [HttpPost("login")]
        public IActionResult UserLogin(UserLoginModel userLogin)
        {
            try
            {
                var result = this.userBL.UserLogin(userLogin);
                if (result != null)
                    return this.Ok(new { success = true, message = "Login Successful", data = result });
                else
                    return this.BadRequest(new { success = false, message = "Sorry! Login Failed", data = result });
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        // Forgot password
        [HttpPost("forgotPassword/{Email}")]
        public IActionResult ForgotPassword(string Email)
        {
            try
            {
                var result = this.userBL.ForgotPassword(Email);
                if (result != null)
                    return this.Ok(new { success = true, message = "Mail Send Successfully", data = result });
                else
                    return this.BadRequest(new { success = false, message = "Sorry! Mail Sending Failed", data = result });
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        // Reset Password
        [Authorize]
        [HttpPut("ResetPassword/{newPassword}/{confirmPassword}")]
        public IActionResult ResetPassword(string newPassword, string confirmPassword)
        {
            try
            {
                var email = User.Claims.FirstOrDefault(e => e.Type == "Email").Value.ToString();
                if (this.userBL.ResetPassword(email, newPassword, confirmPassword))
                {
                    return this.Ok(new { Success = true, message = " Password Changed Successfully " });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = " Password Change Unsuccessfully " });
                }
            }
            catch (System.Exception ex)
            {

                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }

    }
}
