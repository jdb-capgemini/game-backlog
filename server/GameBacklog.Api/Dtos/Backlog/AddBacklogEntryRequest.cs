using GameBacklog.Api.Models;

namespace GameBacklog.Api.Dtos.Backlog;

public class AddBacklogEntryRequest
{
    public int RawgId { get; set; }

    public BacklogStatus Status { get; set; } =
        BacklogStatus.Backlog;
}