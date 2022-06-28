using Common_Layer.Model;
using Microsoft.Extensions.Configuration;
using Repository_Layer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Repository_Layer.Service
{
    public class FeedbackRL : IFeedbackRL
    {
        private SqlConnection sqlConnection;
        private readonly IConfiguration configuration;
        public FeedbackRL(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string AddFeedback(FeedbackModel feedbackModel, int userId)
        {
            this.sqlConnection = new SqlConnection(this.configuration["ConnectionString:BookStore"]);
            try
            {
                using (sqlConnection)
                {
                    SqlCommand cmd = new SqlCommand("AddFeedback", sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Comment", feedbackModel.Comment);
                    cmd.Parameters.AddWithValue("@Rating", feedbackModel.Rating);
                    cmd.Parameters.AddWithValue("@BookId", feedbackModel.BookId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    sqlConnection.Open();
                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    sqlConnection.Close();
                    if (result != 1)
                    {
                        return "Feedback Added";
                    }
                    else
                    {
                        return "Failed";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ViewFeedbackModel> GetFeedback(int BookId)
        {
            this.sqlConnection = new SqlConnection(this.configuration["ConnectionString:BookStore"]);
            try
            {
                using (sqlConnection)
                {
                    SqlCommand cmd = new SqlCommand("GetAllFeedback", sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId", BookId);
                    sqlConnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<ViewFeedbackModel> cartmodels = new List<ViewFeedbackModel>();
                        while (reader.Read())
                        {

                            ViewFeedbackModel cartModel = new ViewFeedbackModel();
                            cartModel.BookId = Convert.ToInt32(reader["BookId"]);
                            cartModel.FullName = reader["FullName"].ToString();
                            cartModel.Comment = reader["Comment"].ToString();


                            cartModel.UserId = Convert.ToInt32(reader["UserId"]);
                            cartModel.FeedbackId = Convert.ToInt32(reader["FeedbackId"]);

                            cartModel.Rating = Convert.ToInt32(reader["Rating"]);
                            //cartModel.AddBookModel = bookModel;
                            cartmodels.Add(cartModel);
                        }

                        sqlConnection.Close();
                        return cartmodels;
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
    }
}
