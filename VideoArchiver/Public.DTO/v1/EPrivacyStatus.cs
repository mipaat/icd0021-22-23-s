namespace Public.DTO.v1;

/// <summary>
/// An archive entity's privacy status.
/// </summary>
public enum EPrivacyStatus
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
    /// <summary>
    /// The video requires authentication/authorization to access?
    /// YouTube-specific?
    /// Not an archive-native privacy status.
    /// </summary>
    NeedsAuth,
    /// <summary>
    /// The video can only be accessed by premium users.
    /// Not an archive-native privacy status.
    /// </summary>
    PremiumOnly,
    /// <summary>
    /// The video can only be accessed by subscribers.
    /// Presumably paid subscribers of the author's channel?
    /// Not an archive-native privacy status.
    /// </summary>
    SubscriberOnly,
}