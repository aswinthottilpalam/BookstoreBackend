using System;
using System.Collections.Generic;
using System.Text;

namespace Common_Layer.Model
{
    public class AddBookModel
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string Rating { get; set; }
        public int TotalReview { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public string BookDetails { get; set; }
        public string BookImage { get; set; }
        public int Quantity { get; set; }
    }
}
