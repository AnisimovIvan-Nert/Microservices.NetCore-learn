var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(WriteCurrentTime);

app.MapGet("/", () => "Hello World!");

app.Run();
return;

async Task WriteCurrentTime(HttpContext context, RequestDelegate next)
{
    Console.WriteLine(DateTime.UtcNow);
    await next(context);
}