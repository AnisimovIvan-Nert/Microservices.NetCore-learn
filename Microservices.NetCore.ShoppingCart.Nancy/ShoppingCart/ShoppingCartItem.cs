namespace Microservices.NetCore.ShoppingCart.Nancy.ShoppingCart;

public class ShoppingCartItem(
    int productCatalogueId,
    string productName,
    string description,
    Money price)
{
    public int ProductCatalogueId { get; } = productCatalogueId;
    public string ProductName { get; } = productName;
    public string Description { get; } = description;
    public Money Price { get; } = price;

    public override bool Equals(object? obj)
    {
        if (obj is not ShoppingCartItem another)
            return false;
        
        return ProductCatalogueId.Equals(another.ProductCatalogueId);
    }
    
    public override int GetHashCode()
    {
        return ProductCatalogueId.GetHashCode();
    }
}