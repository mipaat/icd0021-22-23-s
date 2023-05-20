namespace Public.DTO.v1;

/// <summary>
/// Information about the result of a URL submission.
/// </summary>
public class SubmissionResult
{
    /// <summary>
    /// The ID of the created queue item.
    /// </summary>
    public Guid QueueItemId { get; set; }

    /// <summary>
    /// The type of the submission entity
    /// </summary>
    /// <example>Video</example>
    public EEntityType Type { get; set; }

    /// <summary>
    /// The ID of the submission entity, if it exists.
    /// Present if submission entity already existed in the archive,
    /// or if the submitter was authorized to submit the entity without requiring admin approval.
    /// </summary>
    public Guid? EntityId { get; set; }

    /// <summary>
    /// The platform of the submission entity.
    /// </summary>
    /// <example>YouTube</example>
    public EPlatform Platform { get; set; }

    /// <summary>
    /// The ID of the submission entity on its source platform.
    /// </summary>
    public string IdOnPlatform { get; set; } = default!;

    /// <summary>
    /// Boolean indicating if the entity was already present in the archive. 
    /// </summary>
    public bool AlreadyAdded { get; set; }
}