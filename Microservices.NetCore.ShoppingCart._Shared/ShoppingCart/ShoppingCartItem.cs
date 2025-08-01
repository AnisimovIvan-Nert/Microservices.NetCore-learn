using Microservices.NetCore.ShoppingCart.Shared.ProductClient;

namespace Microservices.NetCore.ShoppingCart.Shared.ShoppingCart;

public class ShoppingCartItem
{
    public int ShoppingCartId { get; }
    public int ProductId { get; }
    public string Name { get; }
    public string Description { get; }
    public Money Price { get; }

    public ShoppingCartItem(int shoppingCartId, ProductCatalogueItem productCatalogueItem)
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productCatalogueItem.Id;
        Name = productCatalogueItem.Name;
        Description = productCatalogueItem.Description;
        Price = productCatalogueItem.Price;
    }
    
    public ShoppingCartItem(int id, int shoppingCartId, int productId, string name, string description, decimal amount, string currency)
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        Name = name;
        Description = description;
        Price = new Money(currency, amount);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not ShoppingCartItem another)
            return false;
        
        return ProductId.Equals(another.ProductId);
    }
    
    public override int GetHashCode()
    {
        return ProductId.GetHashCode();
    }
}