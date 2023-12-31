namespace Public.DTO.v1;

/// <summary>
/// Enum identifying some possible types of errors that can occur when executing an API method.
/// </summary>
public enum EErrorType
{
    /// <summary>
    /// User with provided username is already registered.
    /// </summary>
    UserAlreadyRegistered,
    /// <summary>
    /// Provided credentials for logging in were invalid.
    /// Could mean that the user doesn't exist or that the password was wrong.
    /// </summary>
    InvalidLoginCredentials,
    /// <summary>
    /// Provided details for registering an account were invalid.
    /// </summary>
    InvalidRegistrationData,
    /// <summary>
    /// User account must be approved by an administrator before it can be used.
    /// </summary>
    UserNotApproved,
    /// <summary>
    /// User attempted to perform an action on behalf of an author that isn't a sub-author of the user.
    /// </summary>
    UserAuthorMismatch,

    /// <summary>
    /// Requested token expiration time was invalid.
    /// </summary>
    InvalidTokenExpirationRequested,
    /// <summary>
    /// Provided JWT was invalid.
    /// </summary>
    InvalidJwt,
    /// <summary>
    /// Provided refresh token was invalid.
    /// </summary>
    InvalidRefreshToken,

    /// <summary>
    /// Submitted URL is not recognized/supported by the archive.
    /// </summary>
    UnrecognizedUrl,
}