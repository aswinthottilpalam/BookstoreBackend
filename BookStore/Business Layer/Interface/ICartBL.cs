using Common_Layer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Layer.Interface
{
    public interface ICartBL
    {
        public CartModel AddCart(CartModel cart, int userId);
        public bool RemoveFromCart(int CartId);
        public List<ViewCartModel> GetCartDetailsByUserid(int UserId);
        public CartModel UpdateCart(int CartId, CartModel cartModel, int UserId);
    }
}
