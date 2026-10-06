using System.Text.Json.Serialization;

namespace GameBacklog.Api.Dtos.Rawg;

public class RawgSearchResponse
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("next")]
    public string? Next { get; set; }

    [JsonPropertyName("previous")]
    public string? Previous { get; set; }

    [JsonPropertyName("results")]
    public List<RawgGameSummary> Results { get; set; } = [];
}