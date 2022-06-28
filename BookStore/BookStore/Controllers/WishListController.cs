using Business_Layer.Interface;
using Common_Layer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WishListController : ControllerBase
    {
        private readonly IWishListBL wishBL;

        public WishListController(IWishListBL wishBL)
        {
            this.wishBL = wishBL;
        }

        // Add to wishlist
        [Authorize(Roles = Role.User)]
        [HttpPost("AddWishList")]
        public IActionResult AddWishList(WishListModel wishlistModel)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var cartData = this.wishBL.AddWishList(wishlistModel, userId);
                if (cartData != null)
                {
                    return this.Ok(new { success = true, message = "Book Added SuccessFully to the WishList ", response = cartData });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "SOrry! Failed" });
                }
            }
            catch (System.Exception ex)
            {
                return this.BadRequest(new { Success = false, response = ex.Message });
            }
        }

        // delete from wishlist
        [Authorize(Roles = Role.User)]
        [HttpDelete("DeleteWishList/{WishlistId}")]
        public IActionResult DeleteWishList(int WishlistId)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var result = this.wishBL.DeleteWishList(WishlistId, userId);
                if (result != false)
                {
                    return this.Ok(new { status = true, message = $"Book deleted successFully from Wishlist", Data = result });

                }
                return this.BadRequest(new { status = true, message = $"Sorry! Deletion Failed", Data = result });
            }
            catch (Exception)
            {
                throw;
            }
        }

        // view wishlist
        [Authorize(Roles = Role.User)]
        [HttpPost("GetWishlistDetailsByUserid/{userId}")]
        public IActionResult GetWishlistDetailsByUserid()
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var cartData = this.wishBL.GetWishlistDetailsByUserid(userId);
                if (cartData != null)
                {
                    return this.Ok(new { success = true, message = "Wish List fetched successful ", response = cartData });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Sorry! Failed to fetch Wishlist" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, response = ex.Message });
            }
        }


    }
}
