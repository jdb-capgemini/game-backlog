namespace GameBacklog.Api.Configuration;

public class RawgOptions
{
    public const string SectionName = "Rawg";

    public string BaseUrl { get; set; } = string.Empty;

    public string ApiKey { get; set; } = string.Empty;
}