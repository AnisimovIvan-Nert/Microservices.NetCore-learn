using Nancy;

namespace Microservices.NetCore.HelloWorld.Nancy;

public sealed class HelloWorldModule : NancyModule
{
    public HelloWorldModule()
    {
        Get("/", _ => "Hello World");
    }
}