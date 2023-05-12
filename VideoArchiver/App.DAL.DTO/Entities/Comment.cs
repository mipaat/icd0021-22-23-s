using System.ComponentModel.DataAnnotations.Schema;
using App.DAL.DTO.Base;

namespace App.DAL.DTO.Entities;

public class Comment : BaseArchiveEntityNonMonitored
{
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
    public bool? IsFavorited { get; set; }
    public bool? AuthorIsCreator { get; set; }

    public TimeSpan? CreatedAtVideoTimecode { get; set; }
    public DateTime? DeletedAt { get; set; }

    [InverseProperty(nameof(ReplyTarget))]
    public ICollection<Comment>? DirectReplies { get; set; }
    [InverseProperty(nameof(ConversationRoot))]
    public ICollection<Comment>? ConversationReplies { get; set; }
}