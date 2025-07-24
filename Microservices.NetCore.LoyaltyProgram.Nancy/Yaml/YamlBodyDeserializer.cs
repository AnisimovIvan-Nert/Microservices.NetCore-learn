using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using YamlDotNet.Serialization;

namespace Microservices.NetCore.LoyaltyProgram.Nancy.Yaml;

public class YamlBodyDeserializer : IBodyDeserializer
{
    public bool CanDeserialize(MediaRange mediaRange, BindingContext context) => mediaRange.Subtype.IsYaml();

    public object Deserialize(MediaRange mediaRange, Stream bodyStream, BindingContext context)
    {
        var yamlDeserializer = new Deserializer();
        var reader = new StreamReader(bodyStream);
        return yamlDeserializer.Deserialize(reader, context.DestinationType) ?? throw new InvalidOperationException();
    }
}