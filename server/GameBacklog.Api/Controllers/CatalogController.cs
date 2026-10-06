using GameBacklog.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameBacklog.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/catalog")]
public class CatalogController : ControllerBase
{
    private readonly IRawgClient _rawgClient;

    public CatalogController(IRawgClient rawgClient)
    {
        _rawgClient = rawgClient;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string query,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query) ||
            query.Trim().Length < 2)
        {
            return BadRequest(new
            {
                message =
                    "Enter at least two characters to search."
            });
        }

        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 20);

        try
        {
            var result = await _rawgClient.SearchGamesAsync(
                query.Trim(),
                page,
                pageSize,
                cancellationToken);

            var response = new
            {
                result.Count,
                Page = page,
                PageSize = pageSize,
                Games = result.Results.Select(game => new
                {
                    RawgId = game.Id,
                    game.Name,
                    game.Slug,
                    ReleasedOn = game.Released,
                    game.BackgroundImageUrl,
                    game.MetacriticScore,
                    Platforms = game.Platforms
                        .Select(item => item.Platform.Name)
                        .Distinct()
                        .ToArray(),
                    Genres = game.Genres
                        .Select(genre => genre.Name)
                        .Distinct()
                        .ToArray(),
                    RawgUrl =
                        $"https://rawg.io/games/{game.Slug}"
                })
            };

            return Ok(response);
        }
        catch (HttpRequestException exception)
        {
            return StatusCode(
                StatusCodes.Status502BadGateway,
                new
                {
                    message =
                        "The game catalogue is temporarily unavailable.",
                    detail = exception.Message
                });
        }
    }
}