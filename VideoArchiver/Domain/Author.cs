using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Base;
using Domain.Enums;
using Domain.Identity;
using Domain.NotMapped;

namespace Domain;

public class Author : AbstractIdDatabaseEntity
{
    [MaxLength(64)] public Platform Platform { get; set; } = default!;
    [MaxLength(64)] public string IdOnPlatform { get; set; } = default!;

    public string? UserName { get; set; }
    public string? DisplayName { get; set; }
    public LangString? Bio { get; set; }

    public List<ImageFile>? ProfileImages { get; set; }
    public List<ImageFile>? Banners { get; set; }
    public List<ImageFile>? Thumbnails { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public EPrivacyStatus? PrivacyStatus { get; set; }
    public bool IsAvailable { get; set; }
    public EPrivacyStatus InternalPrivacyStatus { get; set; }

    [MaxLength(4096)] public string? Etag { get; set; }
    public DateTime LastFetched { get; set; }
    public DateTime? LastSuccessfulFetch { get; set; }
    public DateTime AddedToArchiveAt { get; set; }
    public bool Monitor { get; set; }
    public bool Download { get; set; }

    public ICollection<VideoAuthor>? VideoAuthors { get; set; }
    public ICollection<PlaylistAuthor>? PlaylistAuthors { get; set; }
    public ICollection<VideoUploadNotification>? VideoUploadNotifications { get; set; }
    public ICollection<CommentReplyNotification>? CommentReplyNotifications { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<PlaylistSubscription>? PlaylistSubscriptions { get; set; }
    public ICollection<VideoRating>? VideoRatings { get; set; }
    public ICollection<PlaylistRating>? PlaylistRatings { get; set; }
    [InverseProperty(nameof(AuthorRating.Rater))]
    public ICollection<AuthorRating>? AuthorRatings { get; set; }
    [InverseProperty(nameof(AuthorRating.Rated))]
    public ICollection<AuthorRating>? ReceivedAuthorRatings { get; set; }
    public ICollection<Category>? Categories { get; set; }
    public ICollection<AuthorCategory>? AuthorCategories { get; set; }
    public ICollection<StatusChangeEvent>? StatusChangeEvents { get; set; }
    [InverseProperty(nameof(AuthorSubscription.SubscriptionTarget))]
    public ICollection<AuthorSubscription>? SubscriberAuthorSubscriptions { get; set; }
    [InverseProperty(nameof(AuthorSubscription.Subscriber))]
    public ICollection<AuthorSubscription>? SubscribedAuthorSubscriptions { get; set; }
    public ICollection<AuthorPubSub>? AuthorPubSubs { get; set; }
    public ICollection<ExternalUserToken>? ExternalUserTokens { get; set; }
    public ICollection<QueueItem>? QueueItems { get; set; }
}