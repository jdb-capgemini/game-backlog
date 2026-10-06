using System.ComponentModel.DataAnnotations;

namespace GameBacklog.Api.Models;

public class CatalogGame
{
    public int Id { get; set; }

    public int RawgId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Slug { get; set; } = string.Empty;

    public DateOnly? ReleasedOn { get; set; }

    [MaxLength(2000)]
    public string? BackgroundImageUrl { get; set; }

    [MaxLength(2000)]
    public string? RawgUrl { get; set; }

    public int? MetacriticScore { get; set; }

    [MaxLength(1000)]
    public string Platforms { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Genres { get; set; } = string.Empty;

    public DateTime MetadataFetchedAt { get; set; }

    public ICollection<BacklogEntry> BacklogEntries { get; set; }
        = new List<BacklogEntry>();
}