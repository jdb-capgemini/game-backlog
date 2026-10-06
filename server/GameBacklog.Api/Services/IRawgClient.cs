using GameBacklog.Api.Dtos.Rawg;

namespace GameBacklog.Api.Services;

public interface IRawgClient
{
    Task<RawgSearchResponse> SearchGamesAsync(
        string query,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task<RawgGameDetails?> GetGameAsync(
        int rawgId,
        CancellationToken cancellationToken);
}