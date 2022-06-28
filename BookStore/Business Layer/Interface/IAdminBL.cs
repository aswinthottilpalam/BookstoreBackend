using Common_Layer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Layer.Interface
{
    public interface IAdminBL
    {
        public AdminLoginModel AdminLogin(AdminLoginModel adminLogin);
    }
}
