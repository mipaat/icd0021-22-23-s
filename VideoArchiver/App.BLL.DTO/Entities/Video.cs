using App.Common;
using App.Common.Enums;

namespace App.BLL.DTO.Entities;

public class Video
{
    public Guid Id { get; set; }
    public LangString? Title { get; set; }
    public LangString? Description { get; set; }
    public string? Url { get; set; }
    public string? EmbedUrl { get; set; }
    public bool IsAvailable { get; set; }
    public EPrivacyStatus? PrivacyStatus { get; set; }
    public EPrivacyStatus InternalPrivacyStatus { get; set; }

    public long? ViewCount { get; set; }
    public long? LikeCount { get; set; }

    public DateTime? PublishedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public EPlatform Platform { get; set; }
    public string IdOnPlatform { get; set; } = default!;

    public int? CommentCount { get; set; }
    public int ArchivedCommentCount { get; set; }
    public int ArchivedRootCommentCount { get; set; }

    public DateTime? LastFetchOfficial { get; set; }
    public DateTime? LastSuccessfulFetchOfficial { get; set; }
    public DateTime? LastFetchUnofficial { get; set; }
    public DateTime? LastSuccessfulFetchUnofficial { get; set; }
    public DateTime AddedToArchiveAt { get; set; }
    
    public DateTime? LastCommentsFetch { get; set; }
}