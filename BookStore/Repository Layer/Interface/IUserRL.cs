using Common_Layer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Layer.Interface
{
    public interface IUserRL
    {
        public UserRegModel Registration(UserRegModel UserReg);
        public UserLoginModel UserLogin(UserLoginModel userLogin);
        public string ForgotPassword(string Email);
        public bool ResetPassword(string email, string newPassword, string confirmPassword);
    }
}
