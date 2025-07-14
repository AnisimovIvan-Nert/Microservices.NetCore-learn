using Nancy;

namespace Microservices.NetCore.Nancy.HelloWorld;

public sealed class HelloWorldModule : NancyModule
{
    public HelloWorldModule()
    {
        Get("/", _ => "Hello World");
    }
}