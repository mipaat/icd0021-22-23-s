namespace Public.DTO.v1;

/// <summary>
/// The result of a video search query.
/// </summary>
public class VideoSearchResult
{
    /// <summary>
    /// The videos returned by the query.
    /// </summary>
    public List<BasicVideoWithAuthor> Videos { get; set; } = default!;
}