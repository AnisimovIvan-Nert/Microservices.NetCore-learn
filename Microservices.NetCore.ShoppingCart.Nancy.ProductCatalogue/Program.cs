using Microservices.NetCore.Shared.Nancy;
using Nancy.Owin;

var builder = WebApplication.CreateBuilder(args);
builder.AllowSynchronousIO();

var app = builder.Build();

app.UseOwin(pipeline =>
{
    pipeline.UseNancy();
});

app.Run();