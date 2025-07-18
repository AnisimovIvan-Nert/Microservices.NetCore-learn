namespace Microservices.NetCore.ShoppingCart.Nancy.ProductClient;

internal class ProductCatalogueItem
{
    public required string ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string ProductDescription { get; set; }
    public required Money Price { get; set; }
}