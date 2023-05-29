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
    /// Replies to the comment.
    /// Currently the API returns replies only for root comments and flattens them.
    /// </summary>
    public ICollection<Comment> Replies { get; set; } = default!;
}