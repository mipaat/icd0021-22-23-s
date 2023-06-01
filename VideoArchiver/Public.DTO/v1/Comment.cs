namespace Public.DTO.v1;

/// <summary>
/// A comment left on a video.
/// </summary>
public class Comment
{
    /// <summary>
    /// The unique ID of the comment.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The content (text) of the comment.
    /// </summary>
    public string Content { get; set; } = default!;
    /// <summary>
    /// The author of the comment.
    /// </summary>
    public Author Author { get; set; } = default!;
    /// <summary>
    /// When the comment was created.
    /// </summary>
    public DateTime? CreatedAt { get; set; }
    /// <summary>
    /// When the comment was deleted.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
    /// <summary>
    /// Replies to the comment.
    /// Currently the API returns replies only for root comments and flattens them.
    /// </summary>
    public ICollection<Comment> Replies { get; set; } = default!;
    /// <summary>
    /// The amount of upvotes this comment has received.
    /// </summary>
    public int? LikeCount { get; set; }
    /// <summary>
    /// The amount of downvotes this comment has received.
    /// </summary>
    public int? DislikeCount { get; set; }
    /// <summary>
    /// Whether the comment has been marked as favorite by the video author.
    /// </summary>
    public bool? IsFavorited { get; set; }
    
    /// <summary>
    /// When the comment was last fetched using official means.
    /// </summary>
    public DateTime? LastFetchOfficial { get; set; }
    /// <summary>
    /// When the comment was last successfully fetched using official means.
    /// </summary>
    public DateTime? LastSuccessfulFetchOfficial { get; set; }
    /// <summary>
    /// When the comment was last fetched using unofficial means.
    /// </summary>
    public DateTime? LastFetchUnofficial { get; set; }
    /// <summary>
    /// When the comment was last successfully fetched using unofficial means.
    /// </summary>
    public DateTime? LastSuccessfulFetchUnofficial { get; set; }
    /// <summary>
    /// When the comment was first archived.
    /// </summary>
    public DateTime AddedToArchiveAt { get; set; }
}