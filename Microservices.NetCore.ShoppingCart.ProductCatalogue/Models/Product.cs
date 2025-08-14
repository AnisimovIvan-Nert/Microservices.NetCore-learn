namespace Microservices.NetCore.ShoppingCart.ProductCatalogue.Models;

public class Product(int id, string name, string description, Money price)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public string Description { get; } = description;
    public Money Price { get; } = price;
}