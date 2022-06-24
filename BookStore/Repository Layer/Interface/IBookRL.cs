using Common_Layer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Layer.Interface
{
    public interface IBookRL
    {
        public AddBookModel AddBook(AddBookModel book);
        public UpdateBookModel UpdateBook(UpdateBookModel updatebook);
        public bool DeleteBook(int BookId);
        public AddBookModel GetBookByBookId(int BookId);
        public List<AddBookModel> GetAllBooks();
    }
}
