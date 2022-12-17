using FluentValidation;

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

    internal void AssociateCart(Guid cartId)
    {
        CartId = cartId;
    }

    internal decimal CalculateValue()
    {
        return Quantity * Price;
    }

    internal void AddUnity(int unities)
    {
        Quantity += unities;
    }

    internal bool IsValid()
    {
        return new OrderItemValidation().Validate(this).IsValid;
    }

    public class OrderItemValidation : AbstractValidator<CartItem>
    {
        public OrderItemValidation()
        {
            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid Id");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Invalid name");

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .LessThan(5)
                .WithMessage("Min is 1 and max is 5");

            RuleFor(c => c.Price)
                .GreaterThan(0)
                .WithMessage("Item value need to be positive");
        }
    }


}