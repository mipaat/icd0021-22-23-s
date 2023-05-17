using System.ComponentModel.DataAnnotations.Schema;
using App.Common;
using App.Domain.Base;
using App.Domain.Identity;

namespace App.Domain;

public class Author : BaseArchiveEntity
{
    public string? UserName { get; set; }
    public string? DisplayName { get; set; }
    public LangString? Bio { get; set; }

    public long? SubscriberCount { get; set; }

    public ImageFileList? ProfileImages { get; set; }
    public ImageFileList? Banners { get; set; }
    public ImageFileList? Thumbnails { get; set; }

    public Guid? UserId { get; set; }
    public User? User { get; set; }

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
    public ICollection<EntityAccessPermission>? EntityAccessPermissions { get; set; }
}