namespace Public.DTO.v1;

/// <summary>
/// Basic information about a video, including the video's author.
/// </summary>
public class BasicVideoWithAuthor
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
    /// The video's thumbnails.
    /// </summary>
    public List<ImageFile>? Thumbnails { get; set; }
    /// <summary>
    /// A "best" thumbnail selected for the video.
    /// Meaning of "best" may vary.
    /// Likely means highest quality.
    /// </summary>
    public ImageFile? Thumbnail { get; set; }

    /// <summary>
    /// The duration of the video.
    /// </summary>
    public TimeSpan? Duration { get; set; }

    /// <summary>
    /// The video's platform of origin.
    /// </summary>
    public EPlatform Platform { get; set; }
    /// <summary>
    /// The video's ID on it's platform of origin.
    /// </summary>
    public string IdOnPlatform { get; set; } = default!;

    /// <summary>
    /// The author (uploader) of the video.
    /// </summary>
    public Author Author { get; set; } = default!;

    /// <summary>
    /// When the video was created.
    /// </summary>
    public DateTime? CreatedAt { get; set; }
    /// <summary>
    /// When the video was published.
    /// </summary>
    public DateTime? PublishedAt { get; set; }
    /// <summary>
    /// When the video was (first) archived.
    /// </summary>
    public DateTime AddedToArchiveAt { get; set; }
}