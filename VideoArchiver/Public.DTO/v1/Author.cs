namespace Public.DTO.v1;

/// <summary>
/// Basic information about an author.
/// </summary>
public class Author
{
    /// <summary>
    /// The unique ID of the author.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The username of the author on their platform of origin.
    /// </summary>
    public string? UserName { get; set; }
    /// <summary>
    /// The display name of the author on their platform of origin.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// The platform that this author belongs to.
    /// </summary>
    public EPlatform Platform { get; set; }
    /// <summary>
    /// The author's unique ID on their platform of origin.
    /// </summary>
    public string IdOnPlatform { get; set; } = default!;
    /// <summary>
    /// A collection of the author's profile images.
    /// </summary>
    public List<ImageFile>? ProfileImages { get; set; }
    /// <summary>
    /// The URL of the author's page on their platform of origin.
    /// </summary>
    public string? UrlOnPlatform { get; set; }
}