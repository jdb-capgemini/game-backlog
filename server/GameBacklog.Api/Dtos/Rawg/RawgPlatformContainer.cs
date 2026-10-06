using System.Text.Json.Serialization;

namespace GameBacklog.Api.Dtos.Rawg;

public class RawgPlatformContainer
{
    [JsonPropertyName("platform")]
    public RawgNamedResource Platform { get; set; } = new();
}