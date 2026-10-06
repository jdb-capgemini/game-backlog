using GameBacklog.Api.Models;

namespace GameBacklog.Api.Dtos.Backlog;

public record BacklogEntryResponse(
    int Id,
    int RawgId,
    string Name,
    string Slug,
    DateOnly? ReleasedOn,
    string? BackgroundImageUrl,
    string? RawgUrl,
    int? MetacriticScore,
    string[] Platforms,
    string[] Genres,
    BacklogStatus Status,
    int? PersonalRating,
    decimal? EstimatedHours,
    string? Notes,
    DateOnly? StartedOn,
    DateOnly? CompletedOn,
    DateTime CreatedAt,
    DateTime UpdatedAt);