using Public.DTO.v1.Identity;

namespace Public.DTO.v1;

/// <summary>
/// A queue item awaiting approval.
/// </summary>
public class QueueItemForApproval
{
    /// <summary>
    /// The unique ID of the queue item.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The platform of origin of the entity that this queue item is for.
    /// </summary>
    public EPlatform Platform { get; set; }
    /// <summary>
    /// The ID of the entity that this queue item is for on its platform of origin.
    /// </summary>
    public string IdOnPlatform { get; set; } = default!;
    /// <summary>
    /// The type of entity that this queue item is for.
    /// </summary>
    public EEntityType EntityType { get; set; }

    /// <summary>
    /// The user that added this queue item to the archive.
    /// </summary>
    public User AddedBy { get; set; } = default!;
    /// <summary>
    /// The point in time when this queue item was added to the archive.
    /// </summary>
    public DateTime AddedAt { get; set; }

    /// <summary>
    /// Whether the submitter of this queue item should be granted access to its related archive entity,
    /// once the queue item has been approved and processed.
    /// </summary>
    public bool GrantAccess { get; set; }
}