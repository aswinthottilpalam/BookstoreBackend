using Common_Layer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Layer.Interface
{
    public interface IAdminRL
    {
        public AdminLoginModel AdminLogin(AdminLoginModel adminLogin);
    }
}
