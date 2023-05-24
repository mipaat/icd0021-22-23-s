using App.DAL.DTO.Base;

namespace App.DAL.DTO.Entities;

public class CommentRoot : BaseArchiveEntityNonMonitored
{
    public AuthorBasic Author { get; set; } = default!;
    public string? Content { get; set; }
    public int? LikeCount { get; set; }
    public int? DislikeCount { get; set; }
    public int? ReplyCount { get; set; }
    public bool? IsFavorited { get; set; }
    public bool? AuthorIsCreator { get; set; }

    public TimeSpan? CreatedAtVideoTimeCode { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<CommentChild> ConversationReplies { get; set; } = default!;
}