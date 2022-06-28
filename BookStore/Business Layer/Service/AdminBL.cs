using Business_Layer.Interface;
using Common_Layer.Model;
using Repository_Layer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Layer.Service
{
    public class AdminBL : IAdminBL
    {
        private readonly IAdminRL adminRL;

        public AdminBL(IAdminRL adminRL)
        {
            this.adminRL = adminRL;
        }

        public AdminLoginModel AdminLogin(AdminLoginModel adminLogin)
        {
            try
            {
                return this.adminRL.AdminLogin(adminLogin);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
