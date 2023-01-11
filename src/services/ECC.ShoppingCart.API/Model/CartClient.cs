using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace ECC.ShoppingCart.API.Model;

public class CartClient
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<CartItem> Itens { get; set; } = new List<CartItem>();
    public ValidationResult ValidationResult { get; set; }

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


    internal bool IsValid()
    {
        var errors = Itens
            .SelectMany(i => new CartItem.OrderItemValidation().Validate(i).Errors).ToList();
        errors.AddRange(new CartClientValidation().Validate(this).Errors);

        ValidationResult = new ValidationResult(errors);
        return ValidationResult.IsValid;
    }


    internal void UpdateItem(CartItem item)
    {
        
        item.AssociateCart(Id);
        var currentItem = GetProductById(item.ProductId);
        Itens.Remove(currentItem);
        Itens.Add(item);
        CalculateCartValue();
    }

    internal void UpdateUnities(CartItem item, int unity)
    {
        item.UpdateUnity(unity);
        UpdateItem(item);
    }

    internal void RemoveItem(CartItem item)
    {
    
        var currentItem = GetProductById(item.ProductId);
        Itens.Remove(currentItem);
        CalculateCartValue();
    }

    public class CartClientValidation : AbstractValidator<CartClient>
    {
        public CartClientValidation()
        {
            RuleFor(c => c.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("Cliente não reconhecido");

            RuleFor(c => c.Itens.Count)
                .GreaterThan(0)
                .WithMessage("O carrinho não possui itens");

            RuleFor(c => c.TotalPrice)
                .GreaterThan(0)
                .WithMessage("O valor do carrinho precisa ser maior que 0");


        }
    }
}