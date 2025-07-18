using Microservices.NetCore.Shared.Nancy;
using Microservices.NetCore.ShoppingCart.Nancy.ProductClient;
using Nancy.Owin;

var builder = WebApplication.CreateBuilder(args);
builder.AllowSynchronousIO();

builder.Services.AddScoped<IProductCatalogueClient, InMemoryProductCatalogueClient>();

var app = builder.Build();

app.UseOwin(pipeline =>
{
    pipeline.UseNancy();
});

app.Run();