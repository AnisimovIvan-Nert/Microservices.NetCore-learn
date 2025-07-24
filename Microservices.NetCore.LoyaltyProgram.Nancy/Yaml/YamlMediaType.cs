using Nancy.Responses.Negotiation;

namespace Microservices.NetCore.LoyaltyProgram.Nancy.Yaml;

public static class YamlMediaType
{
    public const string Type = "application/" + SybType;
    public const string SybType = "yaml";

    public static bool IsYaml(this MediaType mediaType)
    {
        return mediaType.ToString().EndsWith(SybType);
    }
}