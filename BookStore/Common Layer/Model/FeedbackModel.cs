﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Common_Layer.Model
{
    public class FeedbackModel
    {
        public decimal Rating { get; set; }
        public string Comment { get; set; }
        public int BookId { get; set; }
    }
}
