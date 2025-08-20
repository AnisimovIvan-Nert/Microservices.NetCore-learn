using Microservices.NetCore.ShoppingCart.ProductCatalogue.Services;

namespace Microservices.NetCore.ShoppingCart.ProductCatalogue;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddProductCatalogueServices();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();

        app.Run();
    }
}