namespace Public.DTO.v1;

/// <summary>
/// Data for updating an archive entity's privacy status in the archive.
/// </summary>
public class InternalPrivacyStatusUpdateData
{
    /// <summary>
    /// The privacy status to set the entity to.
    /// </summary>
    public ESimplePrivacyStatus Status { get; set; }
}