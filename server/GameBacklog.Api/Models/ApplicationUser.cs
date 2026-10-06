using System.ComponentModel.DataAnnotations;

namespace GameBacklog.Api.Models;

public class ApplicationUser
{
    public const string LocalUserIdClaimType = "local_user_id";

    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string GoogleSubjectId { get; set; } = string.Empty;

    [Required]
    [MaxLength(320)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string DisplayName { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? PictureUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastLoginAt { get; set; }

    public ICollection<BacklogEntry> BacklogEntries { get; set; }
        = new List<BacklogEntry>();
}