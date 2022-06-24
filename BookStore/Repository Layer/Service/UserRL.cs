using Common_Layer.Model;
using Experimental.System.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository_Layer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Repository_Layer.Service
{
    public class UserRL : IUserRL
    {

        private SqlConnection sqlConnection;
        private readonly IConfiguration configuration;
        public UserRL(IConfiguration configuration)
        {
            this.configuration = configuration;

        }
        private IConfiguration Configuration { get; }

        // Registration 
        public UserRegModel Registration(UserRegModel UserReg)
        {
            sqlConnection = new SqlConnection(this.configuration["ConnectionString:BookStore"]);
            try
            {
                using (sqlConnection)
                {
                    SqlCommand cmd = new SqlCommand("SPUserRegister", sqlConnection);//strore procedure name
                    cmd.CommandType = CommandType.StoredProcedure;
                    var passwordToEncript = EncodePasswordToBase64(UserReg.Password);
                    cmd.Parameters.AddWithValue("@FullName", UserReg.FullName);
                    cmd.Parameters.AddWithValue("@Email", UserReg.Email);
                    cmd.Parameters.AddWithValue("@Password", passwordToEncript);
                    cmd.Parameters.AddWithValue("@MobileNumber", UserReg.MobileNumber);
                    sqlConnection.Open();
                    int result = cmd.ExecuteNonQuery();
                    sqlConnection.Close();
                    if (result != 0)
                    {
                        return UserReg;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // Encrypt Password
        public string EncodePasswordToBase64(string Password)
        {
            try
            {
                byte[] encData_byte = new byte[Password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(Password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }


        // User Login
        public UserLoginModel UserLogin(UserLoginModel userLogin)
        {
            try
            {
                this.sqlConnection = new SqlConnection(this.configuration["ConnectionString:BookStore"]);
                SqlCommand cmd = new SqlCommand("spUserLogin", this.sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                var passwordToEncript = EncodePasswordToBase64(userLogin.Password);
                cmd.Parameters.AddWithValue("@Email", userLogin.Email);
                cmd.Parameters.AddWithValue("@Password", passwordToEncript);

                this.sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    int UserId = 0;
                    UserLoginModel user = new UserLoginModel();
                    while (reader.Read())
                    {
                        user.Email = Convert.ToString(reader["Email"]);
                        user.Password = Convert.ToString(reader["Password"]);
                        UserId = Convert.ToInt32(reader["UserId"]);
                    }
                    this.sqlConnection.Close();
                    user.Token = this.GenerateJWTToken(user.Email, UserId);
                    return user;
                }
                else
                {
                    this.sqlConnection.Close();
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.sqlConnection.Close();
            }
        }

        // Generate JWT token 
        private string GenerateJWTToken(string Email, int UserId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
             {
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim("Email", Email),
                    new Claim("UserId",UserId.ToString())
             }),
                Expires = DateTime.UtcNow.AddHours(24),

                SigningCredentials =
             new SigningCredentials(
                 new SymmetricSecurityKey(tokenKey),
                 SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string ForgotPassword(string Email)
        {
            try
            {
                this.sqlConnection = new SqlConnection(this.configuration["ConnectionString:BookStore"]);
                SqlCommand cmd = new SqlCommand("spUserForgotPassword", this.sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Email", Email);
                this.sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)//HasRows:-Search there is any row or not
                {
                    int UserId = 0;

                    while (reader.Read()) //using while loop for read multiple rows.
                    {
                        Email = Convert.ToString(reader["Email"]);

                        UserId = Convert.ToInt32(reader["UserId"]);
                    }

                    this.sqlConnection.Close();
                    MessageQueue queue;

                    //Add message to Queue
                    if (MessageQueue.Exists(@".\private$\BookStoreQueue"))
                    {
                        queue = new MessageQueue(@".\private$\BookStoreQueue");
                    }
                    else
                    {
                        queue = MessageQueue.Create(@".\private$\BookStoreQueue");
                    }
                    Message Mymessage = new Message();
                    Mymessage.Formatter = new BinaryMessageFormatter();
                    Mymessage.Body = GenerateJWTToken(Email, UserId);
                    Mymessage.Label = "Forgot Password email";
                    queue.Send(Mymessage);

                    Message msg = queue.Receive();
                    msg.Formatter = new BinaryMessageFormatter();
                    EmailServices.SendMail(Email, Mymessage.Body.ToString());
                    queue.ReceiveCompleted += new ReceiveCompletedEventHandler(msmqQueue_ReceiveCompleted);
                    //var Token = this.GenerateJWTToken(Email, UserId);
                    return "Email send successful";
                }
                else
                {
                    this.sqlConnection.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void msmqQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                MessageQueue queue = (MessageQueue)sender;
                Message msg = queue.EndReceive(e.AsyncResult);
                EmailServices.SendMail(e.Message.ToString(), GenerateToken(e.Message.ToString()));
                queue.BeginReceive();

            }
            catch (MessageQueueException ex)
            {
                if (ex.MessageQueueErrorCode == MessageQueueErrorCode.AccessDenied)
                {
                    Console.WriteLine("Access is denied." + "Queue might be a system queue.");
                }
            }
        }

        private string GenerateToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("email", email)
                }),
                Expires = DateTime.UtcNow.AddHours(5),

                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Reset Password
        public bool ResetPassword(string email, string newPassword, string confirmPassword)
        {
            try
            {
                if (newPassword == confirmPassword)
                {
                    this.sqlConnection = new SqlConnection(this.configuration["ConnectionString:BookStore"]);
                    SqlCommand com = new SqlCommand("spUserResetPassword", this.sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    var passwordToEncript = EncodePasswordToBase64(newPassword);
                    com.Parameters.AddWithValue("@Email", email);
                    com.Parameters.AddWithValue("@Password", passwordToEncript);
                    this.sqlConnection.Open();
                    int i = com.ExecuteNonQuery();
                    this.sqlConnection.Close();
                    if (i >= 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                this.sqlConnection.Close();
            }
        }
    }
}
