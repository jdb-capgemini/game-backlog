using System.Text.Json.Serialization;

namespace GameBacklog.Api.Dtos.Rawg;

public class RawgGameDetails : RawgGameSummary
{
    [JsonPropertyName("website")]
    public string? Website { get; set; }

    [JsonPropertyName("description_raw")]
    public string? Description { get; set; }
}