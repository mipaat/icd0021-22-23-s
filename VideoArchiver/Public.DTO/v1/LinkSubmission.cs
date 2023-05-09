namespace Public.DTO.v1;

/// <summary>
/// Data for submitting a link to the archive.
/// </summary>
public class LinkSubmission
{
    /// <summary>
    /// The URL to submit.
    /// </summary>
    /// <remarks>
    /// For example, a YouTube video link: `https://www.youtube.com/watch?v=nU5HbUOtyqk`.
    /// Or a YouTube playlist link: `https://www.youtube.com/playlist?list=PLgRdzXSueEUtWgnFTI77-_E7iawcqqf5f`
    /// </remarks>
    public string Link { get; set; } = default!;
}