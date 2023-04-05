using System.ComponentModel.DataAnnotations;
using Domain.Base;
using Domain.Enums;
using Domain.NotMapped;

namespace Domain;

public class Playlist : AbstractIdDatabaseEntity
{
    public Platform Platform { get; set; } = default!;
    [MaxLength(64)] public string IdOnPlatform { get; set; } = default!;

    public LangString? Title { get; set; }
    public LangString? Description { get; set; }

    public List<ImageFile>? Thumbnails { get; set; }
    public List<ImageFile>? Tags { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public EPrivacyStatus? PrivacyStatus { get; set; }
    public bool IsAvailable { get; set; }
    public EPrivacyStatus InternalPrivacyStatus { get; set; }

    [MaxLength(4096)] public string? Etag { get; set; }
    public DateTime LastFetched { get; set; }
    public DateTime? LastSuccessfulFetch { get; set; }
    public DateTime AddedToArchiveAt { get; set; }
    public bool Monitor { get; set; }
    public bool Download { get; set; }

    public ICollection<PlaylistVideo>? PlaylistVideos { get; set; }
    public ICollection<PlaylistAuthor>? PlaylistAuthors { get; set; }
    public ICollection<PlaylistSubscription>? PlaylistSubscriptions { get; set; }
    public ICollection<PlaylistRating>? PlaylistRatings { get; set; }
    public ICollection<PlaylistCategory>? PlaylistCategories { get; set; }
    public ICollection<PlaylistHistory>? PlaylistHistories { get; set; }
    public ICollection<StatusChangeEvent>? StatusChangeEvents { get; set; }
    public ICollection<QueueItem>? QueueItems { get; set; }
}