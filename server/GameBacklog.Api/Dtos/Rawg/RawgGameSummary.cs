using System.Text.Json.Serialization;

namespace GameBacklog.Api.Dtos.Rawg;

public class RawgGameSummary
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("slug")]
    public string Slug { get; set; } = string.Empty;

    [JsonPropertyName("released")]
    public DateOnly? Released { get; set; }

    [JsonPropertyName("background_image")]
    public string? BackgroundImageUrl { get; set; }

    [JsonPropertyName("metacritic")]
    public int? MetacriticScore { get; set; }

    [JsonPropertyName("platforms")]
    public List<RawgPlatformContainer> Platforms { get; set; } = [];

    [JsonPropertyName("genres")]
    public List<RawgNamedResource> Genres { get; set; } = [];
}