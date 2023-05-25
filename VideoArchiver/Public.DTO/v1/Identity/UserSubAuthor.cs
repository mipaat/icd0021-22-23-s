namespace Public.DTO.v1.Identity;

/// <summary>
/// Basic information about a user's sub-author.
/// </summary>
public class UserSubAuthor
{
    /// <summary>
    /// The unique ID of the author in the archive.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The username of the author.
    /// </summary>
    public string UserName { get; set; } = default!;
    /// <summary>
    /// The display name of the author.
    /// </summary>
    public string? DisplayName { get; set; }
}