using Common_Layer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository_Layer.Interface
{
    public interface IOrderRL
    {
        public string AddOrder(OrderModel orderModel, int userId);

        public List<ViewOrderModel> GetAllOrder(int userId);
    }
}
