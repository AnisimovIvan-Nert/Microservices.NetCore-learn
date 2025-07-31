namespace Microservices.NetCore.ShoppingCart.Shared.ProductCatalogue;

public class Product(int productId, string productName, string description, Money price)
{
    public int ProductId { get; } = productId;
    public string ProductName { get; } = productName;
    public string ProductDescription { get; } = description;
    public Money Price { get; } = price;
}