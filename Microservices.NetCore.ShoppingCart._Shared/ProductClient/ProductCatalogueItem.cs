namespace Microservices.NetCore.ShoppingCart._Shared.ProductClient;

internal class ProductCatalogueItem
{
    public required string ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string ProductDescription { get; set; }
    public required Money Price { get; set; }
}