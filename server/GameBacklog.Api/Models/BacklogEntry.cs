using System.ComponentModel.DataAnnotations;

namespace GameBacklog.Api.Models;

public class BacklogEntry
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public ApplicationUser User { get; set; }

    public int CatalogGameId { get; set; }

    public CatalogGame CatalogGame { get; set; } = null!;

    public BacklogStatus Status { get; set; } = BacklogStatus.Backlog;

    [Range(1, 10)]
    public int? PersonalRating { get; set; }

    [Range(0, 100000)]
    public decimal? EstimatedHours { get; set; }

    [MaxLength(4000)]
    public string? Notes { get; set; }

    public DateOnly? StartedOn { get; set; }

    public DateOnly? CompletedOn { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}