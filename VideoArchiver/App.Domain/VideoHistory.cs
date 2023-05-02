using System.ComponentModel.DataAnnotations;
using App.Domain.Enums;
using App.Domain.NotMapped;
using Domain.Base;

namespace App.Domain;

public class VideoHistory : AbstractIdDatabaseEntity
{
    public Video? Video { get; set; }
    public Guid VideoId { get; set; }

    [MaxLength(64)] public string IdOnPlatform { get; set; } = default!;

    public LangString? Title { get; set; }
    public LangString? Description { get; set; }

    [MaxLength(32)] public string? DefaultLanguage { get; set; }
    [MaxLength(32)] public string? DefaultAudioLanguage { get; set; }

    public TimeSpan? Duration { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? BitrateBps { get; set; }

    public int? ViewCount { get; set; }
    public int? LikeCount { get; set; }
    public int? DislikeCount { get; set; }
    public int? CommentCount { get; set; }

    public CaptionsDictionary? Captions { get; set; }
    public CaptionsDictionary? AutomaticCaptions { get; set; }
    public List<ImageFile>? Thumbnails { get; set; }
    public List<string>? Tags { get; set; }

    public bool? IsLivestreamRecording { get; set; }
    [MaxLength(64)] public string? StreamId { get; set; }
    public DateTime? LivestreamStartedAt { get; set; }
    public DateTime? LivestreamEndedAt { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? RecordedAt { get; set; }

    public List<VideoFile>? LocalVideoFiles { get; set; }

    public DateTime LastValidAt { get; set; }

    public EPrivacyStatus InternalPrivacyStatus { get; set; }
}