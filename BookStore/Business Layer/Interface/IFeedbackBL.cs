using Common_Layer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Layer.Interface
{
    public interface IFeedbackBL
    {
        public string AddFeedback(FeedbackModel feedbackModel, int userId);
        public List<ViewFeedbackModel> GetFeedback(int BookId);
    }
}
