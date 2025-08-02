namespace Microservices.NetCore.ShoppingCart.ProductCatalogue.Models;

public class Product(int productId, string productName, string description, Money price)
{
    public int Id { get; } = productId;
    public string Name { get; } = productName;
    public string Description { get; } = description;
    public Money Price { get; } = price;
}