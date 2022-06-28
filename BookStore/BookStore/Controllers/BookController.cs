using Business_Layer.Interface;
using Common_Layer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController] 
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookBL bookBL;

        public BookController(IBookBL bookBL)
        {
            this.bookBL = bookBL;
        }

        // Add book 
        [Authorize(Roles = Role.Admin)]
        [HttpPost("AddBook")]
        public IActionResult AddBook(AddBookModel book)
        {
            try
            {
                AddBookModel userData = this.bookBL.AddBook(book);
                if (userData != null)
                {
                    return this.Ok(new { Success = true, message = "Book Added Sucessfully", Response = userData });
                }
                return this.Ok(new { Success = true, message = "Sorry! Add book failed" });
            }
            catch (System.Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }

        // Update Book
        [Authorize(Roles = Role.Admin)]
        [HttpPost("UpdateBook")]
        public IActionResult UpdateBook(UpdateBookModel updatebook)
        {
            try
            {
                UpdateBookModel userData = this.bookBL.UpdateBook(updatebook);
                if (userData != null)
                {
                    return this.Ok(new { Success = true, message = "Book Updeted Sucessfully", Response = userData });
                }
                return this.Ok(new { Success = true, message = "Sorry! Book Updation Failed" });
            }
            catch (System.Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }

        // Delete Book
        [Authorize(Roles = Role.Admin)]
        [HttpDelete("DeleteBook/{BookId}")]
        public IActionResult DeleteBook(int BookId)
        {
            try
            {
                if (this.bookBL.DeleteBook(BookId))
                {
                    return this.Ok(new { Success = true, message = "Book Deleted Sucessfully" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Sorry! please Enter Valid Book Id" });
                }
            }
            catch (System.Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }


        // Get book by bookId
        [Authorize]
        [HttpGet("GetBookByBookId/{BookId}")]
        public IActionResult GetBookByBookId(int BookId)
        {
            try
            {
                var book = this.bookBL.GetBookByBookId(BookId);
                if (book != null)
                {
                    return this.Ok(new { Success = true, message = "Book Detail Fetched Sucessfully", Response = book });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Sorry! Please Enter Valid Book Id" });
                }
            }
            catch (System.Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }

        // GetAll Books 
        [Authorize]
        [HttpGet("GetAllBook")]
        public IActionResult GetAllBooks()
        {
            try
            {
                var updatedBookDetail = this.bookBL.GetAllBooks();
                if (updatedBookDetail != null)
                {
                    return this.Ok(new { Success = true, message = "Book Detail Fetched Sucessfully", Response = updatedBookDetail });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Sorry! Wrong credentials" });
                }
            }
            catch (System.Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }


    }
}
