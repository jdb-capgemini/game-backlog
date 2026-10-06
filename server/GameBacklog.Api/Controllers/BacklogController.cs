using System.Security.Claims;
using GameBacklog.Api.Data;
using GameBacklog.Api.Dtos.Backlog;
using GameBacklog.Api.Models;
using GameBacklog.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameBacklog.Api.Controllers;

[Authorize ]
[ApiController]
[Route("api/backlog")]
public class BacklogController : ControllerBase
{
    private readonly GameBacklogDbContext _dbContext;
    private readonly IRawgClient _rawgClient;

    public BacklogController(
        GameBacklogDbContext dbContext,
        IRawgClient rawgClient)
    {
        _dbContext = dbContext;
        _rawgClient = rawgClient;
    }

    [HttpGet]
    public async Task<ActionResult<
        IReadOnlyList<BacklogEntryResponse>>> GetBacklog(
        [FromQuery] BacklogStatus? status,
        [FromQuery] string? search,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var query = _dbContext.BacklogEntries
            .AsNoTracking()
            .Include(entry => entry.CatalogGame)
            .Where(entry => entry.UserId == userId)
            .AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(entry =>
                entry.Status == status.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();

            query = query.Where(entry =>
                entry.CatalogGame.Name.Contains(searchTerm));
        }

        var entries = await query
            .OrderByDescending(entry => entry.UpdatedAt)
            .ToListAsync(cancellationToken);

        return Ok(entries.Select(ToResponse));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BacklogEntryResponse>> GetEntry(
        int id,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var entry = await _dbContext.BacklogEntries
            .AsNoTracking()
            .Include(item => item.CatalogGame)
            .FirstOrDefaultAsync(
                item => item.Id == id && item.UserId == userId,
                cancellationToken);

        if (entry is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(entry));
    }

    [HttpPost]
    public async Task<ActionResult<BacklogEntryResponse>> AddEntry(
        AddBacklogEntryRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        if (request.RawgId <= 0)
        {
            return BadRequest(new
            {
                message = "A valid RAWG game ID is required."
            });
        }

        var existingEntry = await _dbContext.BacklogEntries
            .Include(entry => entry.CatalogGame)
            .FirstOrDefaultAsync(
                entry =>
                    entry.UserId == userId &&
                    entry.CatalogGame.RawgId == request.RawgId,
                cancellationToken);

        if (existingEntry is not null)
        {
            return Conflict(new
            {
                message =
                    "This game is already in your backlog.",
                backlogEntryId = existingEntry.Id
            });
        }

        var catalogGame = await _dbContext.CatalogGames
            .FirstOrDefaultAsync(
                game => game.RawgId == request.RawgId,
                cancellationToken);

        if (catalogGame is null)
        {
            var rawgGame = await _rawgClient.GetGameAsync(
                request.RawgId,
                cancellationToken);

            if (rawgGame is null)
            {
                return NotFound(new
                {
                    message =
                        "The selected RAWG game could not be found."
                });
            }

            catalogGame = new CatalogGame
            {
                RawgId = rawgGame.Id,
                Name = rawgGame.Name,
                Slug = rawgGame.Slug,
                ReleasedOn = rawgGame.Released,
                BackgroundImageUrl =
                    rawgGame.BackgroundImageUrl,
                RawgUrl =
                    $"https://rawg.io/games/{rawgGame.Slug}",
                MetacriticScore =
                    rawgGame.MetacriticScore,
                Platforms = JoinNames(
                    rawgGame.Platforms.Select(
                        item => item.Platform.Name)),
                Genres = JoinNames(
                    rawgGame.Genres.Select(
                        genre => genre.Name)),
                MetadataFetchedAt = DateTime.UtcNow
            };

            _dbContext.CatalogGames.Add(catalogGame);
        }

        var now = DateTime.UtcNow;

        var backlogEntry = new BacklogEntry
        {
            UserId = userId,
            CatalogGame = catalogGame,
            Status = request.Status,
            CreatedAt = now,
            UpdatedAt = now
        };

        _dbContext.BacklogEntries.Add(backlogEntry);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(
            nameof(GetEntry),
            new { id = backlogEntry.Id },
            ToResponse(backlogEntry));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateEntry(
        int id,
        UpdateBacklogEntryRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        if (request.StartedOn.HasValue &&
            request.CompletedOn.HasValue &&
            request.CompletedOn < request.StartedOn)
        {
            ModelState.AddModelError(
                nameof(request.CompletedOn),
                "The completion date cannot be before the start date.");

            return ValidationProblem(ModelState);
        }

        var entry = await _dbContext.BacklogEntries
            .FirstOrDefaultAsync(
                e => e.Id == id && e.UserId == userId,
                cancellationToken);

        if (entry is null)
        {
            return NotFound();
        }

        entry.Status = request.Status;
        entry.PersonalRating = request.PersonalRating;
        entry.EstimatedHours = request.EstimatedHours;
        entry.Notes = Normalise(request.Notes);
        entry.StartedOn = request.StartedOn;
        entry.CompletedOn = request.CompletedOn;
        entry.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEntry(
        int id,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var entry = await _dbContext.BacklogEntries
            .FirstOrDefaultAsync(
                e => e.Id == id && e.UserId == userId,
                cancellationToken);

        if (entry is null)
        {
            return NotFound();
        }

        _dbContext.BacklogEntries.Remove(entry);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    private bool TryGetCurrentUserId(out int userId)
    {
        var value = User.FindFirstValue(
            ApplicationUser.LocalUserIdClaimType);

        return int.TryParse(value, out userId);
    }

    private static BacklogEntryResponse ToResponse(
        BacklogEntry entry)
    {
        return new BacklogEntryResponse(
            entry.Id,
            entry.CatalogGame.RawgId,
            entry.CatalogGame.Name,
            entry.CatalogGame.Slug,
            entry.CatalogGame.ReleasedOn,
            entry.CatalogGame.BackgroundImageUrl,
            entry.CatalogGame.RawgUrl,
            entry.CatalogGame.MetacriticScore,
            SplitNames(entry.CatalogGame.Platforms),
            SplitNames(entry.CatalogGame.Genres),
            entry.Status,
            entry.PersonalRating,
            entry.EstimatedHours,
            entry.Notes,
            entry.StartedOn,
            entry.CompletedOn,
            entry.CreatedAt,
            entry.UpdatedAt);
    }

    private static string JoinNames(IEnumerable<string> values)
    {
        return string.Join(
            ", ",
            values
                .Where(value =>
                    !string.IsNullOrWhiteSpace(value))
                .Distinct(StringComparer.OrdinalIgnoreCase));
    }

    private static string[] SplitNames(string value)
    {
        return value.Split(
            ',',
            StringSplitOptions.TrimEntries |
            StringSplitOptions.RemoveEmptyEntries);
    }

    private static string? Normalise(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}