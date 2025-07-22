using Nancy;
using Nancy.Bootstrapper;

namespace Microservices.NetCore.LoyaltyProgram.Nancy;

public class Bootstrapper : DefaultNancyBootstrapper
{
    protected override Func<ITypeCatalog, NancyInternalConfiguration> InternalConfiguration
        => NancyInternalConfiguration.WithOverrides(builder => builder.StatusCodeHandlers.Clear());
}