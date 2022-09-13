namespace ECC.ShoppingCart.API.Model;

public class CartItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Image { get; set; }
    public Guid CartId { get; set; }
    public CartClient CartClient { get; set; }

    public CartItem()
    {
       
    }
    
}