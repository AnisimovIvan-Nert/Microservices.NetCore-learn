namespace Microservices.NetCore.ShoppingCart.Shared.ProductClient;

public class ProductCatalogueItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required Money Price { get; init; }
}