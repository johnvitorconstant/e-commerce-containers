namespace ECC.ShoppingCart.API.Model;

public class CartClient
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<CartItem> Itens { get; set; } = new List<CartItem>();

    public CartClient(Guid clientId)
    {
        ClientId = clientId;
    }

    public CartClient()
    {
        
    }

}