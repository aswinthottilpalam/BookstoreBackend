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
    public class AddressController : ControllerBase
    {
        private readonly IAddressBL addressBL;

        public AddressController(IAddressBL addressBL)
        {
            this.addressBL = addressBL;
        }

        //Add Addresss
        [Authorize(Roles = Role.User)]
        [HttpPost("AddAddress")]
        public IActionResult AddAddress(AddAddressModel addAddress)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var cartData = this.addressBL.AddAddress(addAddress, userId);
                if (cartData != null)
                {
                    return this.Ok(new { success = true, message = "Address Added SuccessFully", response = cartData });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Address adding Failed" });
                }
            }
            catch (System.Exception ex)
            {
                return this.BadRequest(new { Success = false, response = ex.Message });
            }
        }

        // Delete Address
        [Authorize(Roles = Role.User)]
        [HttpDelete("DeleteAddress/{AddressId}")]
        public IActionResult DeleteAddress(int AddressId)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var cartData = this.addressBL.DeleteAddress(AddressId, userId);
                if (cartData != null)
                {
                    return this.Ok(new { success = true, message = "Address Deleted SuccessFully", response = cartData });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Address Deletion Failed" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, response = ex.Message });
            }
        }

        //Update Address
        [Authorize(Roles = Role.User)]
        [HttpPost("UpdateAddress")]
        public IActionResult UpdateAddress(AddressModel addressModel)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                var cartData = this.addressBL.UpdateAddress(addressModel, userId);
                if (cartData != null)
                {
                    return this.Ok(new { success = true, message = "Address Updated SuccessFully", response = cartData });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Address updation Failed" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, response = ex.Message });
            }
        }

    }
}
