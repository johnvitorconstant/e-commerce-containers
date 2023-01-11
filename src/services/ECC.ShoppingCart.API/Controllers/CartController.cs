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

            if (cart == null) ManipulateNewCart(item);
            
            else ManipulateExistingCart(cart, item);
            
            ValidateCart(cart);

            if(!OperationIsValid()) return CustomResponse();

            await PersistData();
            return CustomResponse();

        }




        [HttpPut("cart/{productId}")]
        public async Task<IActionResult> UpdateCartItem(Guid productId, CartItem item)
        {

            var cart = await GetClientCart();
            var cartItem = await GetValidatedCartItem(productId, cart, item);
            if (cartItem == null) return CustomResponse();

            cart.UpdateUnities(cartItem, item.Quantity);

            ValidateCart(cart);
            if(!OperationIsValid()) return CustomResponse();

            _context.CartItens.Update(cartItem);
            _context.CartClients.Update(cart);

            await PersistData();


            return CustomResponse();

        }

        [HttpDelete("cart/{productId}")]
        public async Task<IActionResult> RemoveCartItem(Guid productId)
        {
            var cart = await GetClientCart();
            var cartItem = await GetValidatedCartItem(productId, cart);
            if (cartItem == null)
            {
                return CustomResponse();
            }

            ValidateCart(cart);
            if(!OperationIsValid()) return CustomResponse();

            cart.RemoveItem(cartItem);
            _context.CartItens.Remove(cartItem);
            _context.CartClients.Update(cart);
            await PersistData();

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


        private async Task<CartClient> GetClientCart()
        {
            return await _context.CartClients
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.ClientId == _user.GetUserId());
        }


        private async Task<CartItem> GetValidatedCartItem(Guid productId, CartClient cart, CartItem item=null)
        {
            if (item!=null && productId != item.ProductId)
            {
                AddProcessError("O item não corresponde ao informado");
                return null;
            }

            if (cart == null)
            {
                AddProcessError("Carrinho não encontrado");
                return null;
            }

            var cartItem = await _context.CartItens.FirstOrDefaultAsync(c => c.CartId == cart.Id && c.ProductId == productId);

            if (cartItem == null || !cart.CartItemExist(cartItem))
            {
                AddProcessError("O item não está no carrinho");
                return null;
            }

            return cartItem;

        }

        private async Task PersistData()
        {
            var result = await _context.SaveChangesAsync();
            if (result <= 0) AddProcessError("Houve um erro no banco de dados");


        }

        private bool ValidateCart(CartClient cart)
        {
            if(cart.IsValid()) return true;

            cart.ValidationResult.Errors.ToList().ForEach(e => AddProcessError(e.ErrorMessage));
            return false;
        }
    }
}
