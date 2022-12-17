using ECC.ShoppingCart.API.Model;
using ECC.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECC.ShoppingCart.API.Controllers
{
    [Authorize]
    public class CartController : MainController
    {
        [HttpGet("cart")]
        public async Task<CartClient> GetCart()
        {
            return null;
        }

        [HttpPost("cart")]
        public async Task<IActionResult> AddItemCart(CartItem item)
        {
            return CustomResponse();
        }

        [HttpPut("cart/{productId}")]
        public async Task<IActionResult> UpdateCartItem(Guid productId, CartItem item)
        {
            return CustomResponse();
        }

        [HttpDelete("cart/{productId}")]
        public async Task<IActionResult> RemoveCartItem(Guid productId)
        {
            return CustomResponse();
        }


    }
}
