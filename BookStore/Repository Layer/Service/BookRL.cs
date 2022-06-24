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
    public class BookRL : IBookRL
    {
        private SqlConnection sqlConnection;
        private readonly IConfiguration configuration;
        public BookRL(IConfiguration configuration)
        {
            this.configuration = configuration;

        }
        private IConfiguration Configuration { get; }

        // Add Book Details 
        public AddBookModel AddBook(AddBookModel book)
        {
            sqlConnection = new SqlConnection(this.configuration["ConnectionString:BookStore"]);
            try
            {
                using (sqlConnection)
                {
                    SqlCommand cmd = new SqlCommand("spAddBook", sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookName", book.BookName);
                    cmd.Parameters.AddWithValue("@authorName", book.AuthorName);
                    cmd.Parameters.AddWithValue("@rating", book.Rating);
                    cmd.Parameters.AddWithValue("@totalReview", book.TotalReview);
                    cmd.Parameters.AddWithValue("@originalPrice", book.OriginalPrice);
                    cmd.Parameters.AddWithValue("@discountPrice", book.DiscountPrice);
                    cmd.Parameters.AddWithValue("@BookDetails", book.BookDetails);
                    cmd.Parameters.AddWithValue("@bookImage", book.BookImage);
                    cmd.Parameters.AddWithValue("@Quantity", book.Quantity);
                    sqlConnection.Open();
                    int result = cmd.ExecuteNonQuery();
                    sqlConnection.Close();
                    if (result != 0)
                    {
                        return book;
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

        // Update Book details 
        public UpdateBookModel UpdateBook(UpdateBookModel updatebook)
        {
            sqlConnection = new SqlConnection(this.configuration["ConnectionString:BookStore"]);
            try
            {
                using (sqlConnection)
                {
                    SqlCommand cmd = new SqlCommand("spUpdateBook", sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookId", updatebook.BookId);
                    cmd.Parameters.AddWithValue("@BookName", updatebook.BookName);
                    cmd.Parameters.AddWithValue("@authorName", updatebook.AuthorName);
                    cmd.Parameters.AddWithValue("@rating", updatebook.Rating);
                    cmd.Parameters.AddWithValue("@totalReview", updatebook.TotalReview);
                    cmd.Parameters.AddWithValue("@originalPrice", updatebook.OriginalPrice);
                    cmd.Parameters.AddWithValue("@discountPrice", updatebook.DiscountPrice);
                    cmd.Parameters.AddWithValue("@BookDetails", updatebook.BookDetails);
                    cmd.Parameters.AddWithValue("@bookImage", updatebook.BookImage);
                    cmd.Parameters.AddWithValue("@Quantity", updatebook.Quantity);
                    sqlConnection.Open();
                    int result = cmd.ExecuteNonQuery();
                    sqlConnection.Close();
                    if (result != 0)
                    {
                        return updatebook;
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

        // Delete Book..
        public bool DeleteBook(int BookId)
        {
            sqlConnection = new SqlConnection(this.configuration["ConnectionString:BookStore"]);
            try
            {
                using (sqlConnection)
                {
                    SqlCommand cmd = new SqlCommand("spDeleteBook", sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@BookId", BookId);
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

        public AddBookModel GetBookByBookId(int BookId)
        {
            try
            {
                this.sqlConnection = new SqlConnection(this.configuration["ConnectionString:BookStore"]);
                SqlCommand cmd = new SqlCommand("spGetBookByBookId", this.sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@BookId", BookId);
                this.sqlConnection.Open();
                AddBookModel bookModel = new AddBookModel();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        bookModel.BookName = reader["BookName"].ToString();
                        bookModel.AuthorName = reader["AuthorName"].ToString();
                        bookModel.Rating = reader["Rating"].ToString();
                        bookModel.TotalReview = Convert.ToInt32(reader["TotalReview"]);
                        bookModel.OriginalPrice = Convert.ToInt32(reader["OriginalPrice"]);
                        bookModel.DiscountPrice = Convert.ToInt32(reader["DiscountPrice"]);
                        bookModel.BookDetails = reader["BookDetails"].ToString();
                        bookModel.BookImage = reader["BookImage"].ToString();
                        bookModel.Quantity = Convert.ToInt32(reader["Quantity"]);
                    }

                    this.sqlConnection.Close();
                    return bookModel;
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

        public List<AddBookModel> GetAllBooks()
        {
            try
            {
                List<AddBookModel> book = new List<AddBookModel>();
                this.sqlConnection = new SqlConnection(this.configuration["ConnectionString:BookStore"]);
                SqlCommand cmd = new SqlCommand("spGetAllBooks", this.sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                this.sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        book.Add(new AddBookModel
                        {
                            BookId = Convert.ToInt32(reader["BookId"]),
                            BookName = reader["BookName"].ToString(),
                            AuthorName = reader["AuthorName"].ToString(),
                            Rating = reader["Rating"].ToString(),
                            TotalReview = Convert.ToInt32(reader["TotalReview"]),

                            OriginalPrice = Convert.ToDecimal(reader["OriginalPrice"]),
                            DiscountPrice = Convert.ToDecimal(reader["DiscountPrice"]),

                            BookDetails = reader["BookDetails"].ToString(),
                            BookImage = reader["bookImage"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                        });
                    }

                    this.sqlConnection.Close();
                    return book;
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
    }
}
