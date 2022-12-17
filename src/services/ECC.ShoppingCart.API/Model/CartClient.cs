using Microsoft.AspNetCore.Mvc;

namespace ECC.ShoppingCart.API.Model;

public class CartClient
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<CartItem> Itens { get; set; } = new List<CartItem>();

    public CartClient(Guid clientId)

    {
        Id = Guid.NewGuid();
        ClientId = clientId;
    }

    public CartClient()
    {

    }


    internal CartItem GetProductById(Guid productId)
    {
        return Itens.FirstOrDefault(p => p.ProductId == productId);
    }

    internal void CalculateCartValue()
    {
        TotalPrice = Itens.Sum(p => p.CalculateValue());
    }

    internal bool CartItemExist(CartItem item)
    {
        return Itens.Any(p => p.ProductId == item.ProductId);
    }


    internal void AddItem(CartItem item)
    {
        if(!item.IsValid()) return;
        item.AssociateCart(Id);

        if (CartItemExist(item))
        {
            var currentItem = GetProductById(item.ProductId);
            currentItem.AddUnity(item.Quantity);

            item = currentItem;
            Itens.Remove(currentItem);

        }
        Itens.Add(item);
        CalculateCartValue();


    }

}