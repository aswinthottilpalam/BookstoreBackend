using Business_Layer.Interface;
using Common_Layer.Model;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminBL adminBL;

        public AdminController(IAdminBL adminBL)
        {
            this.adminBL = adminBL;
        }


        [HttpPost("AdminLogin")]
        public IActionResult AdminLogin(AdminLoginModel adminLogin)
        {
            try
            {
                var result = this.adminBL.AdminLogin(adminLogin);
                if (result != null)
                    return this.Ok(new { success = true, message = "Admin Login Successful", data = result });
                else
                    return this.BadRequest(new { success = false, message = "Sorry!Admin Login Failed", data = result });
            }
            catch (System.Exception)
            {
                throw;
            }
        }


    }
}
