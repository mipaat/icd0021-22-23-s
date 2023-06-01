namespace Public.DTO.v1;

/// <summary>
/// Information about a video.
/// </summary>
public class VideoWithAuthor
{
    /// <summary>
    /// The unique ID of the video.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The video's title.
    /// </summary>
    public LangString? Title { get; set; }
    /// <summary>
    /// The video's description.
    /// </summary>
    public LangString? Description { get; set; }
    /// <summary>
    /// The video's URL on its platform of origin.
    /// </summary>
    public string? Url { get; set; }
    /// <summary>
    /// The URL to embed a video player from the video's platform of origin.
    /// </summary>
    public string? EmbedUrl { get; set; }
    /// <summary>
    /// Whether the video is available on its platform of origin.
    /// </summary>
    public bool IsAvailable { get; set; }
    /// <summary>
    /// The video's privacy status on its platform of origin.
    /// </summary>
    public EPrivacyStatus? PrivacyStatus { get; set; }
    /// <summary>
    /// The video's privacy status in the archive.
    /// </summary>
    public ESimplePrivacyStatus InternalPrivacyStatus { get; set; }

    /// <summary>
    /// How many views the video has on its platform of origin.
    /// </summary>
    public long? ViewCount { get; set; }
    /// <summary>
    /// How many likes the video has on its platform of origin.
    /// </summary>
    public long? LikeCount { get; set; }
    
    /// <summary>
    /// When the video was published on its platform of origin.
    /// </summary>
    public DateTime? PublishedAt { get; set; }
    /// <summary>
    /// When the video was created.
    /// Exact semantic meaning may vary.
    /// May be the same as PublishedAt.
    /// </summary>
    public DateTime? CreatedAt { get; set; }
    /// <summary>
    /// When the video was last updated.
    /// May not apply to all information about the video.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// The video's platform of origin - the platform that the video belongs to.
    /// </summary>
    public EPlatform Platform { get; set; }
    /// <summary>
    /// The video's ID on its platform of origin.
    /// </summary>
    public string IdOnPlatform { get; set; } = default!;
    
    /// <summary>
    /// How many comments the video has on its platform of origin.
    /// </summary>
    public int? CommentCount { get; set; }
    /// <summary>
    /// How many total comments have been archived for this video.
    /// </summary>
    public int ArchivedCommentCount { get; set; }
    /// <summary>
    /// How many root comments (comments that aren't replying to another comment) have been archived for this video.
    /// </summary>
    public int ArchivedRootCommentCount { get; set; }

    /// <summary>
    /// The author (uploader) of the video.
    /// </summary>
    public Author Author { get; set; } = default!;

    /// <summary>
    /// When the video was last fetched using official means.
    /// </summary>
    public DateTime? LastFetchOfficial { get; set; }
    /// <summary>
    /// When the video was last successfully fetched using official means.
    /// </summary>
    public DateTime? LastSuccessfulFetchOfficial { get; set; }
    /// <summary>
    /// When the video was last fetched using unofficial means.
    /// </summary>
    public DateTime? LastFetchUnofficial { get; set; }
    /// <summary>
    /// When the video was last successfully fetched using unofficial means.
    /// </summary>
    public DateTime? LastSuccessfulFetchUnofficial { get; set; }
    /// <summary>
    /// When the video was (first) archived.
    /// </summary>
    public DateTime AddedToArchiveAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the video's comments were last fetched.
    /// </summary>
    public DateTime? LastCommentsFetch { get; set; }
}