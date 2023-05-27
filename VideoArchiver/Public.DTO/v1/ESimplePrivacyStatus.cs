namespace Public.DTO.v1;

/// <summary>
/// An archive entity's privacy status,
/// excluding unusual/unnecessary/platform-specific values.
/// </summary>
public enum ESimplePrivacyStatus
{
    /// <summary>
    /// The video is publicly accessible without requiring authentication.
    /// </summary>
    Public,
    /// <summary>
    /// The video is unlisted. It can be accessed via its URL,
    /// but it won't show up in listings for users without the necessary permissions.
    /// </summary>
    Unlisted,
    /// <summary>
    /// The video is private and can only be accessed by authorized users.
    /// </summary>
    Private,
}