using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Enums;
using Domain.Base;

namespace App.Domain;

public class Comment : AbstractIdDatabaseEntity
{
    public Platform Platform { get; set; } = default!;
    [MaxLength(64)] public string IdOnPlatform { get; set; } = default!;

    public Author? Author { get; set; }
    public Guid AuthorId { get; set; }
    public Video? Video { get; set; }
    public Guid VideoId { get; set; }
    [ForeignKey(nameof(ReplyTargetId))]
    public Comment? ReplyTarget { get; set; }
    public Guid? ReplyTargetId { get; set; }
    [ForeignKey(nameof(ConversationRootId))]
    public Comment? ConversationRoot { get; set; }
    public Guid? ConversationRootId { get; set; }

    public string? Content { get; set; }

    public int? LikeCount { get; set; }
    public int? DislikeCount { get; set; }
    public int? ReplyCount { get; set; }

    public DateTime? CreatedAt { get; set; }
    public TimeSpan? CreatedAtVideoTimecode { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public EPrivacyStatus? PrivacyStatus { get; set; }
    public bool IsAvailable { get; set; }
    public EPrivacyStatus InternalPrivacyStatus { get; set; }

    [MaxLength(4096)] public string? Etag { get; set; }
    public DateTime LastFetched { get; set; }
    public bool FetchSuccess { get; set; }
    public DateTime AddedToArchiveAt { get; set; }

    [InverseProperty(nameof(ReplyTarget))]
    public ICollection<Comment>? DirectReplies { get; set; }
    [InverseProperty(nameof(ConversationRoot))]
    public ICollection<Comment>? ConversationReplies { get; set; }
}