namespace Public.DTO.v1;

/// <summary>
/// Necessary parameters for approving a queue item.
/// </summary>
public class ApproveQueueItemInput
{
    /// <summary>
    /// The ID of the queue item being approved.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Boolean indicating whether or not the user who submitted the queue item
    /// should be granted access to the archive entity related to this queue item.
    /// </summary>
    public bool GrantAccess { get; set; } = true;
}