using System.ComponentModel.DataAnnotations;
using App.Common.Enums;
using Domain.Base;

namespace App.DAL.DTO.Base;

public abstract class BaseArchiveEntityNonMonitored : AbstractIdDatabaseEntity
{
    public EPlatform Platform { get; set; } = default!;
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
}