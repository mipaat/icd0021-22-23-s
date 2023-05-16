namespace Public.DTO.v1;

/// <summary>
/// Information about the result of a URL submission.
/// </summary>
public class SubmissionResult
{
    /// <summary>
    /// The type of the added entity, if one was added.
    /// </summary>
    /// <example>Video</example>
    public EEntityType? Type { get; set; }
    /// <summary>
    /// The platform of the added or queued entity.
    /// </summary>
    /// <example>YouTube</example>
    public EPlatform? Platform { get; set; }
    /// <summary>
    /// The unique ID of the added entity or queue item (in the archive, not on its source platform).
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Boolean indicating if the submission result is an archive entity or a queue entry awaiting approval.
    /// </summary>
    public bool IsQueueItem { get; set; }
    /// <summary>
    /// Boolean indicating if the entity was already present in the archive.
    /// In this case, the entity referred to by this submission result's ID is that previously added entity. 
    /// </summary>
    public bool AlreadyAdded { get; set; }
}
