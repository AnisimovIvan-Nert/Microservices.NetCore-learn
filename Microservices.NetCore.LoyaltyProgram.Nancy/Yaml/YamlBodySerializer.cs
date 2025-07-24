using Nancy;
using Nancy.Responses.Negotiation;
using YamlDotNet.Serialization;

namespace Microservices.NetCore.LoyaltyProgram.Nancy.Yaml;

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