using System.Net;
using System.Net.Http.Json;
using GameBacklog.Api.Configuration;
using GameBacklog.Api.Dtos.Rawg;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace GameBacklog.Api.Services;

public class RawgClient : IRawgClient
{
    private readonly HttpClient _httpClient;
    private readonly RawgOptions _options;
    private readonly ILogger<RawgClient> _logger;

    public RawgClient(
        HttpClient httpClient,
        IOptions<RawgOptions> options,
        ILogger<RawgClient> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<RawgSearchResponse> SearchGamesAsync(
        string query,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        EnsureConfigured();

        var parameters = new Dictionary<string, string?>
        {
            ["key"] = _options.ApiKey,
            ["search"] = query,
            ["page"] = page.ToString(),
            ["page_size"] = pageSize.ToString(),
            ["search_precise"] = "true"
        };

        var requestUri = QueryHelpers.AddQueryString(
            "games",
            parameters);

        using var response = await _httpClient.GetAsync(
            requestUri,
            cancellationToken);

        await EnsureSuccessfulResponseAsync(
            response,
            cancellationToken);

        return await response.Content
                   .ReadFromJsonAsync<RawgSearchResponse>(
                       cancellationToken: cancellationToken)
               ?? new RawgSearchResponse();
    }

    public async Task<RawgGameDetails?> GetGameAsync(
        int rawgId,
        CancellationToken cancellationToken)
    {
        EnsureConfigured();

        var requestUri = QueryHelpers.AddQueryString(
            $"games/{rawgId}",
            "key",
            _options.ApiKey);

        using var response = await _httpClient.GetAsync(
            requestUri,
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        await EnsureSuccessfulResponseAsync(
            response,
            cancellationToken);

        return await response.Content
            .ReadFromJsonAsync<RawgGameDetails>(
                cancellationToken: cancellationToken);
    }

    private void EnsureConfigured()
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new InvalidOperationException(
                "The RAWG API key has not been configured.");
        }
    }

    private async Task EnsureSuccessfulResponseAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var content = await response.Content.ReadAsStringAsync(
            cancellationToken);

        _logger.LogWarning(
            "RAWG returned status code {StatusCode}. Response: {Response}",
            response.StatusCode,
            content);

        throw new HttpRequestException(
            $"RAWG returned HTTP {(int)response.StatusCode}.",
            null,
            response.StatusCode);
    }
}