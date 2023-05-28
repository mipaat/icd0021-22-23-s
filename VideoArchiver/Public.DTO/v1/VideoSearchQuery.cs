namespace Public.DTO.v1;

/// <summary>
/// Parameters for filtering, sorting, and paginating a video search query.
/// </summary>
public class VideoSearchQuery
{
    /// <summary>
    /// IDs of the categories that the selected videos must belong to.
    /// Currently ORed together (may be changed to AND later).
    /// </summary>
    public Guid[]? CategoryIdsQuery { get; set; } = null;
    /// <summary>
    /// The ID of the author that the query is being made on behalf of.
    /// If specified, used for including private category assignments made by this author.
    /// </summary>
    public Guid? UserAuthorId { get; set; }

    /// <summary>
    /// Only include videos from this platform.
    /// </summary>
    public EPlatform? PlatformQuery { get; set; }
    /// <summary>
    /// Only include videos with titles containing this string.
    /// Searches all translations of the videos' titles.
    /// </summary>
    public string? NameQuery { get; set; }
    /// <summary>
    /// Only include videos with authors whose names contain this string.
    /// </summary>
    public string? AuthorQuery { get; set; }

    /// <summary>
    /// The page of the result set to get.
    /// </summary>
    public int Page { get; set; } = 0;
    /// <summary>
    /// The amount of results to fetch per page.
    /// Can't be higher than 100.
    /// </summary>
    public int Limit { get; set; } = 50;
    /// <summary>
    /// Which attribute to sort the result set by.
    /// </summary>
    public EVideoSortingOptions SortingOptions { get; set; } = EVideoSortingOptions.CreatedAt;
    /// <summary>
    /// Whether the result set should be sorted in descending order.
    /// </summary>
    public bool Descending { get; set; } = true;
}