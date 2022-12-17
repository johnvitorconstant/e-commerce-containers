using ECC.ShoppingCart.API.Data;
using ECC.ShoppingCart.API.Model;
using ECC.WebAPI.Core.Controllers;
using ECC.WebAPI.Core.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECC.ShoppingCart.API.Controllers
{
    [Authorize]
    public class CartController : MainController


    {

        private readonly IAspNetUser _user;
        private readonly ShoppingCartContext _context;

        public CartController(IAspNetUser user, ShoppingCartContext context)
        {
            _user = user;
            _context = context;
        }



        [HttpGet("cart")]
        public async Task<CartClient> GetCart()
        {

            return await GetClientCart() ?? new CartClient();
        }

        [HttpPost("cart")]
        public async Task<IActionResult> AddItemCart(CartItem item)
        {
            var cart = await GetCart();

            if (cart == null)
            {
                ManipulateNewCart(item);
            }
            else
            {
                ManipulateExistingCart(cart, item);
            }

            if (!OperationIsValid())
            {
                return CustomResponse();
            }

            var result = await _context.SaveChangesAsync();
            if(result<=0) AddProcessError("Houve um erro no banco de dados");
            return CustomResponse();

        }

        private void ManipulateNewCart(CartItem item)
        {
            var cart = new CartClient(_user.GetUserId());
            cart.AddItem(item);

            _context.CartClients.Add(cart);
        }

        private void ManipulateExistingCart(CartClient cart, CartItem item)
        {
            var productItemExisting = cart.CartItemExist(item);
            cart.AddItem(item);

            if (productItemExisting)
            {
                _context.CartItens.Update(cart.GetProductById(item.ProductId));
            }
            else
            {
                _context.CartItens.Add(item);
            }

            _context.CartClients.Update(cart);
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


        private async Task<CartClient> GetClientCart()
        {
            return await _context.CartClients
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.ClientId == _user.GetUserId());
        }

    }
}
