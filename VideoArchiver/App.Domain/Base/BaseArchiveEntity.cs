using System.ComponentModel.DataAnnotations;
using App.Domain.Enums;
using Domain.Base;

namespace App.Domain.Base;

public abstract class BaseArchiveEntity : AbstractIdDatabaseEntity
{
    public Platform Platform { get; set; } = default!;
    [MaxLength(64)] public string IdOnPlatform { get; set; } = default!;

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public EPrivacyStatus? PrivacyStatus { get; set; }
    public bool IsAvailable { get; set; }
    public EPrivacyStatus InternalPrivacyStatus { get; set; } = EPrivacyStatus.Private;

    [MaxLength(4096)] public string? Etag { get; set; }
    public DateTime? LastFetchOfficial { get; set; }
    public DateTime? LastSuccessfulFetchOfficial { get; set; }
    public DateTime? LastFetchUnofficial { get; set; }
    public DateTime? LastSuccessfulFetchUnofficial { get; set; }
    public DateTime AddedToArchiveAt { get; set; } = DateTime.UtcNow;

    public bool Monitor { get; set; } = true;
    public bool Download { get; set; } = true;
}