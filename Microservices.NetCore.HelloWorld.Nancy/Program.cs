using Microservices.NetCore.Shared.Nancy;
using Nancy.Owin;

var builder = WebApplication.CreateBuilder(args);
builder.AllowSynchronousIO();

var app = builder.Build();

app.UseOwin(pipeline =>
{
    pipeline(next => environment =>
    {
        Console.WriteLine("Got request");
        return next(environment);
    });
    pipeline.UseNancy();
});

app.Run();


