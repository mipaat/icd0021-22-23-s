using System.ComponentModel.DataAnnotations;
using App.Domain.Enums;
using App.Domain.NotMapped;
using Domain.Base;

namespace App.Domain;

public class Video : AbstractIdDatabaseEntity
{
    public Platform Platform { get; set; } = default!;
    [MaxLength(64)] public string IdOnPlatform { get; set; } = default!;

    public LangString? Title { get; set; }
    public LangString? Description { get; set; }

    [MaxLength(32)] public string? DefaultLanguage { get; set; }
    [MaxLength(32)] public string? DefaultAudioLanguage { get; set; }

    public TimeSpan? Duration { get; set; }

    public long? ViewCount { get; set; }
    public long? LikeCount { get; set; }
    public long? DislikeCount { get; set; }
    public long? CommentCount { get; set; }

    public bool? HasCaptions { get; set; }
    public List<Caption>? Captions { get; set; }
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

    public ICollection<VideoGame>? VideoGames { set; get; }
    public ICollection<VideoAuthor>? VideoAuthors { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<VideoRating>? VideoRatings { get; set; }
    public ICollection<VideoCategory>? VideoCategories { get; set; }
    public ICollection<StatusChangeEvent>? StatusChangeEvents { get; set; }
    public ICollection<VideoHistory>? VideoHistories { get; set; }
}