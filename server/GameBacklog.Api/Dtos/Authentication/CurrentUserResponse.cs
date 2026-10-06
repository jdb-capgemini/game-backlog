namespace GameBacklog.Api.Dtos.Authentication;

public record CurrentUserResponse(
    string Id,
    string DisplayName,
    string Email,
    string? PictureUrl);