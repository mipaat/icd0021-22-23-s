namespace Public.DTO.v1;

/// <summary>
/// Values for specifying a property to sort a collection of videos by.
/// </summary>
public enum EVideoSortingOptions
{
    /// <summary>
    /// The time a video was created or published at.
    /// Exact interpretation may change/vary.
    /// May not be an exact match to a video's CreatedAt property.
    /// </summary>
    CreatedAt,
    /// <summary>
    /// The duration of a video.
    /// </summary>
    Duration,
}