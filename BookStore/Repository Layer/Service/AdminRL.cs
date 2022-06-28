using Common_Layer.Model;
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
    public class AdminRL : IAdminRL
    {
        private SqlConnection sqlConnection;
        private readonly IConfiguration configuration;
        public AdminRL(IConfiguration configuration)
        {
            this.configuration = configuration;

        }
        private IConfiguration Configuration { get; }

        public AdminLoginModel AdminLogin(AdminLoginModel adminLogin)
        {
            try
            {
                this.sqlConnection = new SqlConnection(this.configuration["ConnectionString:BookStore"]);
                SqlCommand cmd = new SqlCommand("AdminLogin", this.sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Email", adminLogin.Email);
                cmd.Parameters.AddWithValue("@Password", adminLogin.Password);

                this.sqlConnection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
            
                if (reader.HasRows)//HasRows:-Search there is any row or not
                {
                    int AdminId = 0;
                    AdminLoginModel admin = new AdminLoginModel();
                    while (reader.Read()) 
                    {
                        admin.Email = Convert.ToString(reader["Email"]);
                        admin.Password = Convert.ToString(reader["Password"]);
                        AdminId = Convert.ToInt32(reader["AdminId"]);


                    }

                    this.sqlConnection.Close();
                    admin.Token = this.GenerateJWTTokenForAdmin(adminLogin, AdminId);
                    return admin;
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

        private string GenerateJWTTokenForAdmin(AdminLoginModel adminLogin, int AdminId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim("Email",adminLogin.Email),
                    new Claim("AdminId",AdminId.ToString())
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

    }
}
