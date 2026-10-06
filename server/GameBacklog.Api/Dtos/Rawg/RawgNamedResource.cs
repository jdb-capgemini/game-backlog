using System.Text.Json.Serialization;

namespace GameBacklog.Api.Dtos.Rawg;

public class RawgNamedResource
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;
}