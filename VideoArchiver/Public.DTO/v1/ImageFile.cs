namespace Public.DTO.v1;

/// <summary>
/// Information about an image file.
/// </summary>
public class ImageFile
{
    /// <summary>
    /// A string identifying the image's type.
    /// For example, in the case of a thumbnail, it might distinguish between
    /// the main version and alternative auto-generated versions.
    /// Varies by platform and may not be present.
    /// </summary>
    /// <example>default</example>
    /// <example>1</example>
    /// <example>2</example>
    /// <example>3</example>
    public string? Key { get; set; }
    /// <summary>
    /// A string identifying the image file's quality.
    /// Varies by platform and may not be set.
    /// </summary>
    /// <example>medium</example>
    /// <example>high</example>
    /// <example>standard</example>
    /// <example>maxres</example>
    public string? Quality { get; set; }
    /// <summary>
    /// The URL where the image file can be accessed.
    /// </summary>
    public string Url { get; set; } = default!;
    /// <summary>
    /// The width of the image in pixels.
    /// </summary>
    public int? Width { get; set; }
    /// <summary>
    /// The height of the image in pixels.
    /// </summary>
    public int? Height { get; set; }
}