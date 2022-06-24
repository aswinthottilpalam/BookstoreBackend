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
    public class WishListRL : IWishListRL
    {
        private SqlConnection sqlConnection;
        private readonly IConfiguration configuration;
        public WishListRL(IConfiguration configuration)
        {
            this.configuration = configuration;

        }
        private IConfiguration Configuration { get; }

        // Add to Wishlist
        public WishListModel AddWishList(WishListModel wishlistModel, int userId)
        {
            try
            {
                this.sqlConnection = new SqlConnection(this.configuration["ConnectionString:BookStore"]);
                SqlCommand cmd = new SqlCommand("spAddWishList", this.sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@BookId", wishlistModel.BookId);
                this.sqlConnection.Open();
                int i = cmd.ExecuteNonQuery();
                this.sqlConnection.Close();
                if (i >= 1)
                {
                    return wishlistModel;
                }
                else
                {
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

        // Delete book from wishlist
        public bool DeleteWishList(int WishlistId, int userId)
        {
            sqlConnection = new SqlConnection(this.configuration["ConnectionString:BookStore"]);
            try
            {
                using (sqlConnection)
                {
                    SqlCommand cmd = new SqlCommand("spDeleteWishList", sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@WishListId", WishlistId);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    sqlConnection.Open();
                    int result = cmd.ExecuteNonQuery();
                    sqlConnection.Close();
                    if (result != 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // View wishlist
        public List<ViewWishListModel> GetWishlistDetailsByUserid(int userId)
        {
            sqlConnection = new SqlConnection(this.configuration["ConnectionString:BookStore"]);
            try
            {
                using (sqlConnection)
                {
                    SqlCommand cmd = new SqlCommand("spGetWishListByUserId", sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    sqlConnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        List<ViewWishListModel> Wishlistmodels = new List<ViewWishListModel>();
                        while (reader.Read())
                        {
                            AddBookModel bookModel = new AddBookModel();
                            ViewWishListModel WishlistModel = new ViewWishListModel();
                            bookModel.BookId = Convert.ToInt32(reader["BookId"]);
                            bookModel.BookName = reader["BookName"].ToString();
                            bookModel.AuthorName = reader["AuthorName"].ToString();
                            bookModel.OriginalPrice = Convert.ToInt32(reader["OriginalPrice"]);
                            bookModel.DiscountPrice = Convert.ToInt32(reader["DiscountPrice"]);
                            bookModel.BookImage = reader["BookImage"].ToString();
                            WishlistModel.userId = Convert.ToInt32(reader["UserId"]);
                            WishlistModel.bookId = Convert.ToInt32(reader["BookId"]);
                            WishlistModel.WishlistId = Convert.ToInt32(reader["WishListId"]);
                            WishlistModel.Bookmodel = bookModel;
                            Wishlistmodels.Add(WishlistModel);
                        }
                        sqlConnection.Close();
                        return Wishlistmodels;
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
