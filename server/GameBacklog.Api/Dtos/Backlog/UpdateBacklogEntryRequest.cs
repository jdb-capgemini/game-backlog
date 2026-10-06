using System.ComponentModel.DataAnnotations;
using GameBacklog.Api.Models;

namespace GameBacklog.Api.Dtos.Backlog;

public class UpdateBacklogEntryRequest
{
    public BacklogStatus Status { get; set; }

    [Range(1, 10)]
    public int? PersonalRating { get; set; }

    [Range(0, 100000)]
    public decimal? EstimatedHours { get; set; }

    [MaxLength(4000)]
    public string? Notes { get; set; }

    public DateOnly? StartedOn { get; set; }

    public DateOnly? CompletedOn { get; set; }
}