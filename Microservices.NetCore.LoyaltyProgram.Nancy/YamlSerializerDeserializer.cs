using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using YamlDotNet.Serialization;

namespace Microservices.NetCore.LoyaltyProgram.Nancy;

public static class YamlMediaType
{
    public const string Type = "application/" + SybType;
    public const string SybType = "yaml";

    public static bool IsYaml(this MediaType mediaType)
    {
        return mediaType.ToString().EndsWith(SybType);
    }
}

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

public class YamlBodySerializer : IResponseProcessor
{
    public IEnumerable<Tuple<string, MediaRange>> ExtensionMappings =>
    [
        new(YamlMediaType.SybType, new MediaRange(YamlMediaType.Type))
    ];

    public ProcessorMatch CanProcess(MediaRange requestedMediaRange, dynamic model, NancyContext context)
    {
        var yamlProcessorMatch = new ProcessorMatch
        {
            ModelResult = MatchResult.DontCare,
            RequestedContentTypeResult = MatchResult.NonExactMatch
        };

        return requestedMediaRange.Subtype.IsYaml()
            ? yamlProcessorMatch
            : ProcessorMatch.None;
    }

    public Response Process(MediaRange requestedMediaRange, dynamic model, NancyContext context)
    {
        return new Response
        {
            Contents = Contents,
            ContentType = YamlMediaType.Type
        };

        void Contents(Stream stream)
        {
            var yamlSerializer = new Serializer();
            var streamWriter = new StreamWriter(stream);
            yamlSerializer.Serialize(streamWriter, model);
            streamWriter.Flush();
        }
    }
}